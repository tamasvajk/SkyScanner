// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Console
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using NodaTime;

    using SkyScanner.Booking;
    using SkyScanner.Data;
    using SkyScanner.Data.Interim;
    using SkyScanner.Services;
    using SkyScanner.Settings;

    static class Program {
        static void Main()
        {
            WriteLine("Detailed search:");
            SearchDetailed().Wait();
            WriteSeparator(2);
            WriteLine("Simplified search:");
            SearchSimplified().Wait();
        }

        private static async Task SearchSimplified()
        {
            var scanner = new Scanner(ConfigurationManager.AppSettings["apiKey"]);
            var from = (await scanner.QueryLocation("London")).First();
            var to = (await scanner.QueryLocation("New York")).First();

            var now = new LocalDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var outboundDate = now.PlusWeeks(1);
            var inboundDate = now.PlusWeeks(2);

            //Query flights
            var itineraries = await scanner.QueryFlight(
                new FlightQuerySettings(
                    new FlightRequestSettings(from, to, outboundDate, inboundDate),
                    new FlightResponseSettings(SortType.Price, SortOrder.Ascending)),
                WriteToDebug());

            itineraries = itineraries
                .Take(5)
                .ToList();

            if (!itineraries.Any())
            {
                WriteLine("No flights");
                return;
            }

            foreach (var itinerary in itineraries)
            {
                WriteItinerary(itinerary, Currency.Default);
            }

            WriteSeparator();

            //Query bookings (note, this is forbidden by SkyScanner, should only query exact booking details if a user requests them)
            var bookingQueryTasks = itineraries.Select(i => scanner.QueryBooking(i));
            var bookingResults = (await Task.WhenAll(bookingQueryTasks))
                .OrderBy(response =>
                    response.BookingOptions
                        .Select(option => option.BookingItems.Sum(item => item.Price))
                        .Min());

            foreach (var response in bookingResults)
            {
                WriteBookingResult(response, Currency.Default);
            }
        }

        private static async Task SearchDetailed()
        {
            //Initialize Scanner
            var scanner = new Scanner(
                ConfigurationManager.AppSettings["apiKey"],
                RetryExecutionStrategy.Default);

            //Query locales
            var locales = await scanner.QueryLocale();
            var currentLocale = locales.FirstOrDefault(locale => locale.Name.StartsWith("English"));

            if (currentLocale == null)
            {
                WriteLine(ErrorColor, "Couldn't find locale, using default instead");
                currentLocale = Locale.Default;
            }

            //Query markets
            var markets = await scanner.QueryMarket(currentLocale);
            var currentMarket = markets.FirstOrDefault(market => market.Name == "Switzerland");

            if (currentMarket == null)
            {
                WriteLine(ErrorColor, "Couldn't find market, using default instead");
                currentMarket = Market.Default;
            }

            //Query currencies
            var currencies = await scanner.QueryCurrency();
            var currentCurrency = currencies.FirstOrDefault(currency => currency.Code == "CHF");

            if (currentCurrency == null)
            {
                WriteLine(ErrorColor, "Couldn't find currency, using default instead");
                currentCurrency = Currency.Default;
            }

            //Query location
            const string fromPlaceName = "London";
            var from = (await scanner.QueryLocation(new LocationAutosuggestSettings(fromPlaceName,
                LocationAutosuggestQueryType.Query, currentMarket, currentCurrency, currentLocale))).First();
            if (from == null)
            {
                WriteLine(ErrorColor, "Couldn't find '{0}'", fromPlaceName);
                return;
            }

            //Query destination location
            const string toPlaceName = "New York";
            var to = (await scanner.QueryLocation(new LocationAutosuggestSettings(toPlaceName,
                LocationAutosuggestQueryType.Query, currentMarket, currentCurrency, currentLocale))).FirstOrDefault();
            if (to == null)
            {
                WriteLine(ErrorColor, "Couldn't find '{0}'", toPlaceName);
                return;
            }

            //Setup flight search settings
            var flightResponseSettings = new FlightResponseSettings(
                sortOrder: SortOrder.Ascending,
                sortType: SortType.Price,
                maxStops: 2,
                maxDuration: 14 * 60,
                outboundDepartureStartTime: new LocalTime(08, 0, 0),
                outboundDepartureEndTime: new LocalTime(12, 0, 0),
                inboundDepartureStartTime: new LocalTime(08, 0, 0),
                inboundDepartureEndTime: new LocalTime(18, 30, 0)
                );

            var now = new LocalDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var outboundDate = now.PlusWeeks(1);
            var inboundDate = now.PlusWeeks(2);

            Write("Flights from ");
            Write(ImportantColor, from.PlaceName);
            Write(" to ");
            WriteLine(ImportantColor, to.PlaceName);
            Write(" on ");
            Write(ImportantColor, outboundDate.ToString("d", CultureInfo.InvariantCulture));
            Write(" and back on ");
            WriteLine(ImportantColor, inboundDate.ToString("d", CultureInfo.InvariantCulture));

            //Query flights
            var itineraries = await scanner.QueryFlight(
                new FlightQuerySettings(
                    new FlightRequestSettings(from, to, outboundDate, inboundDate, 1,
                        currency: currentCurrency, marketCountry: currentMarket, locale: currentLocale),
                    flightResponseSettings), WriteToDebug());

            itineraries = itineraries
                .Take(5)
                .ToList();

            if (!itineraries.Any())
            {
                WriteLine("No flights");
                return;
            }

            foreach (var itinerary in itineraries)
            {
                WriteItinerary(itinerary, currentCurrency);
            }

            WriteLine("----------------------------------------------");

            //Query bookings (note, this is forbidden by SkyScanner, should only query exact booking details if a user requests them)
            var bookingQueryTasks = itineraries.Select(i => scanner.QueryBooking(i));
            var bookingResults = (await Task.WhenAll(bookingQueryTasks))
                .OrderBy(response =>
                    response.BookingOptions
                        .Select(option => option.BookingItems.Sum(item => item.Price))
                        .Min());

            foreach (var response in bookingResults)
            {
                WriteBookingResult(response, currentCurrency);
            }
        }

        private static Action<InterimChangeSet<Itinerary>> WriteToDebug()
        {
            return WriteToDebug;
        }

        private static void WriteToDebug(InterimChangeSet<Itinerary> list)
        {
            try
            {
                var leg = list.All.First().OutboundLeg;
            }
            catch(Exception e)
            {
                throw new InvalidDataException("The interim result contains legs that are NULL - this should not happen", e);
            }

            Debug.WriteLine($"Interim results recieved ! {list.All.Count()} "
                                       + $"total itineraries, {list.Additions.Count()} new "
                                       + $"and {list.Updates.Count()} updates. IsLast? " + list.IsLastChangeSet);
        }

        private static void WriteBookingResult(BookingResponse response, Currency currentCurrency)
        {
            var price = response.BookingOptions.Select(option => option.BookingItems.Sum(item => item.Price)).Min();
            Write("  price: ");
            //Format price according to currency
            WriteLine(ImportantColor, currentCurrency.FormatValue(price));
            WriteLegs(response.Itinerary);
        }

        private static void WriteItinerary(Itinerary itinerary, Currency currentCurrency)
        {
            var price = itinerary.PricingOptions.Min(option => option.Price);
            var pricingOption = itinerary.PricingOptions.First(option => option.Price == price);

            Write("  price: ");
            //Format price according to currency
            WriteLine(ImportantColor, currentCurrency.FormatValue(price));
            Write("  age: ");
            WriteLine(ImportantColor, pricingOption.QuoteAge + "min");
            WriteLegs(itinerary);
        }

        private static void WriteLegs(Itinerary itinerary)
        {
            WriteLine("   outbound itinerary at {1} with {0} ({2}-{3})",
                string.Join(", ", itinerary.OutboundLeg.OperatingCarriers.Select(c => c.Name)),
                itinerary.OutboundLeg.DepartureTime.TimeOfDay,
                itinerary.OutboundLeg.Origin.Code,
                itinerary.OutboundLeg.Destination.Code);
            WriteLine("   inbound itinerary at {1} with {0} ({2}-{3})",
                string.Join(", ", itinerary.InboundLeg.OperatingCarriers.Select(c => c.Name)),
                itinerary.InboundLeg.DepartureTime.TimeOfDay,
                itinerary.InboundLeg.Origin.Code,
                itinerary.InboundLeg.Destination.Code);
        }

        #region Write helpers

        private static void Write(ConsoleColor color, string format, params object[] arg)
        {
            var prevConsoleColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            Write(format, arg);
            System.Console.ForegroundColor = prevConsoleColor;
        }
        private static void WriteLine(ConsoleColor color, string format, params object[] arg)
        {
            Write(color, format + Environment.NewLine, arg);
        }
        private static void WriteLine(string format, params object[] arg)
        {
            Write(format + Environment.NewLine, arg);
        }
        private static void Write(string format, params object[] arg)
        {
            //write to Console for the time being
            System.Console.Write(format, arg);
        }

        private static void WriteSeparator(int number = 1)
        {
            for (int i = 0; i < number; i++)
            {
                WriteLine("----------------------------------------------");
            }
        }

        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor ImportantColor = ConsoleColor.Green;

        #endregion
    }
}
