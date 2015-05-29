// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using SkyScanner.Booking;
using SkyScanner.Data;
using SkyScanner.Services;
using SkyScanner.Settings;

namespace SkyScanner.Console
{
    using Console = System.Console;
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Detailed search:");
            SearchDetailed().Wait();
            WriteSeparator(2);
            Console.WriteLine("Simplified search:");
            SearchSimplified().Wait();
            Console.ReadKey();
        }

        private static async Task SearchSimplified()
        {
            var scanner = new Scanner(ConfigurationManager.AppSettings["apiKey"]);
            var from = (await scanner.QueryLocation("London")).First();
            var to = (await scanner.QueryLocation("New York")).First();

            //Query flights
            var itineraries = await scanner.QueryFlight(
                new FlightQuerySettings(
                    new FlightRequestSettings(from, to, new LocalDate(2015, 06, 19), new LocalDate(2015, 06, 25)),
                    new FlightResponseSettings(SortType.Price, SortOrder.Ascending)));

            itineraries = itineraries
                .Take(5)
                .ToList();

            if (!itineraries.Any())
            {
                Console.WriteLine("No flights");
                return;
            }

            foreach (var itinerary in itineraries)
            {
                WriteItinerary(itinerary, Currency.Default);
            }

            WriteSeparator();

            //Query bookings (note, this is forbidden by SkyScanner, should only query exact booking details if a user requests them)
            var bookingQueryTasks = itineraries.Select(scanner.QueryBooking);
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
                WriteErrorLine("Couldn't find locale, using default instead");
                currentLocale = Locale.Default;
            }

            //Query markets
            var markets = await scanner.QueryMarket(currentLocale);
            var currentMarket = markets.FirstOrDefault(market => market.Name == "Switzerland");

            if (currentMarket == null)
            {
                WriteErrorLine("Couldn't find market, using default instead");
                currentMarket = Market.Default;
            }

            //Query currencies
            var currencies = await scanner.QueryCurrency();
            var currentCurrency = currencies.FirstOrDefault(currency => currency.Code == "CHF");

            if (currentCurrency == null)
            {
                WriteErrorLine("Couldn't find currency, using default instead");
                currentCurrency = Currency.Default;
            }
            
            //Query location
            const string fromPlaceName = "London";
            var from = (await scanner.QueryLocation(new LocationAutosuggestSettings(fromPlaceName,
                LocationAutosuggestQueryType.Query, currentMarket, currentCurrency, currentLocale))).First();
            if (from == null)
            {
                WriteErrorLine("Couldn't find '{0}'", fromPlaceName);
                return;
            }

            //Query destination location
            const string toPlaceName = "New York";
            var to = (await scanner.QueryLocation(new LocationAutosuggestSettings(toPlaceName,
                LocationAutosuggestQueryType.Query, currentMarket, currentCurrency, currentLocale))).FirstOrDefault();
            if (to == null)
            {
                WriteErrorLine("Couldn't find '{0}'", toPlaceName);
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

            var outboundDate = new LocalDate(2015, 06, 19);
            var inboundDate = new LocalDate(2015, 06, 25);

            Console.Write("Flights from ");
            WriteImportant(from.PlaceName);
            Console.Write(" to ");
            WriteImportantLine(to.PlaceName);
            Console.Write(" on ");
            WriteImportant(outboundDate.ToString("d", CultureInfo.InvariantCulture));
            Console.Write(" and back on ");
            WriteImportantLine(inboundDate.ToString("d", CultureInfo.InvariantCulture));
            
            //Query flights
            var itineraries = await scanner.QueryFlight(
                new FlightQuerySettings(
                    new FlightRequestSettings(from, to, outboundDate, inboundDate, 1,
                        currency: currentCurrency, marketCountry: currentMarket, locale: currentLocale),
                    flightResponseSettings));

            itineraries = itineraries
                .Take(5)
                .ToList();

            if (!itineraries.Any())
            {
                Console.WriteLine("No flights");
                return;
            }

            foreach (var itinerary in itineraries)
            {
                WriteItinerary(itinerary, currentCurrency);
            }

            Console.WriteLine("----------------------------------------------");

            //Query bookings (note, this is forbidden by SkyScanner, should only query exact booking details if a user requests them)
            var bookingQueryTasks = itineraries.Select(scanner.QueryBooking);
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

        private static void WriteBookingResult(BookingResponse response, Currency currentCurrency)
        {
            var price = response.BookingOptions.Select(option => option.BookingItems.Sum(item => item.Price)).Min();
            Console.Write("  price: ");
            //Format price according to currency
            WriteImportantLine(currentCurrency.FormatValue(price));
            WriteLegs(response.Itinerary);
        }

        private static void WriteItinerary(Itinerary itinerary, Currency currentCurrency)
        {
            var price = itinerary.PricingOptions.Min(option => option.Price);
            var pricingOption = itinerary.PricingOptions.First(option => option.Price == price);

            Console.Write("  price: ");
            //Format price according to currency
            WriteImportantLine(currentCurrency.FormatValue(price));
            Console.Write("  age: ");
            WriteImportantLine(pricingOption.QuoteAge + "min");
            WriteLegs(itinerary);
        }

        private static void WriteLegs(Itinerary itinerary)
        {
            Console.WriteLine("   outbound itinerary at {1} with {0} ({2}-{3}, {4} stop{5})",
                string.Join(", ", itinerary.OutboundLeg.OperatingCarriers.Select(c => c.Name)),
                itinerary.OutboundLeg.DepartureTime.TimeOfDay,
                itinerary.OutboundLeg.Origin.Code,
                itinerary.OutboundLeg.Destination.Code,
                itinerary.OutboundLeg.Stops.Count,
                itinerary.OutboundLeg.Stops.Count == 1 ? "" : "s");
            Console.WriteLine("   inbound itinerary at {1} with {0} ({2}-{3}, {4} stop{5})",
                string.Join(", ", itinerary.InboundLeg.OperatingCarriers.Select(c => c.Name)),
                itinerary.InboundLeg.DepartureTime.TimeOfDay,
                itinerary.InboundLeg.Origin.Code,
                itinerary.InboundLeg.Destination.Code,
                itinerary.InboundLeg.Stops.Count,
                itinerary.InboundLeg.Stops.Count == 1 ? "" : "s");
        }

        #region Write helpers

        private static void WriteSeparator(int number = 1)
        {
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine("----------------------------------------------");
            }
        }
        private static void WriteErrorLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text, args);
            Console.ResetColor();
        }
        private static void WriteImportant(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(text, args);
            Console.ResetColor();
        }
        private static void WriteImportantLine(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text, args);
            Console.ResetColor();
        }

        #endregion
    }
}
