// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;
using SkyScanner.Booking;
using SkyScanner.Data;
using SkyScanner.Services.Base;
using SkyScanner.Services.Helpers;
using SkyScanner.Settings;

namespace SkyScanner.Services
{
    /// <summary>
    /// The facade to query all SkyScanner services
    /// </summary>
    public sealed class Scanner : IScanner
    {
        private readonly string _apiKey;
        private readonly IExecutionStrategy _executionStrategy;

        /// <summary>
        /// Initializes a new instance of the Scanner with the default query execution strategy
        /// </summary>
        /// <param name="apiKey">API key to access SkyScanner services</param>
        public Scanner(string apiKey)
            :this (apiKey, new DefaultExecutionStrategy())
        {
        }

        /// <summary>
        /// Initializes a new instance of the Scanner
        /// </summary>
        /// <param name="apiKey">API key to access SkyScanner services</param>
        /// <param name="executionStrategy">Query execution strategy</param>
        public Scanner(string apiKey, IExecutionStrategy executionStrategy)
        {
            _apiKey = apiKey;
            _executionStrategy = executionStrategy;
        }
        
        internal static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new SkyScannerContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(),
                new NodaPatternConverter<LocalTime>(LocalTimePattern.Create("HH:mm", CultureInfo.InvariantCulture))
            }
        }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        /// <summary>
        /// The Live Pricing Service (flight query) returns all the flights available for a specific route and 
        /// date (or single date for one-way searches).
        /// </summary>
        /// <param name="flightQuerySettings">Settings for the query</param>
        /// <returns></returns>
        public async Task<List<Itinerary>> QueryFlight(FlightQuerySettings flightQuerySettings)
        {
            var flightService = new Flight(_apiKey, flightQuerySettings);
            return await _executionStrategy.Execute(async () =>
            {
                var pinger = await flightService.SendQuery();
                var response = await pinger.SendQuery();
                return response.Itineraries;
            });
        }

        /// <summary>
        /// The Booking Details service can be used to drill down into an itinerary and get its full details, including 
        /// the necessary deeplinks to actually make the booking.
        /// </summary>
        /// <param name="itinerary">The itinerary for which the booking details should be queried</param>
        /// <returns></returns>
        public async Task<BookingResponse> QueryBooking(Itinerary itinerary)
        {
            var settings = new BookingQuerySettings(
                new BookingRequestSettings(itinerary.FlightResponse.SessionKey, itinerary),
                itinerary);

            var bookingService = new Booking(_apiKey, settings);
            return await _executionStrategy.Execute(async () =>
            {
                var pinger = await bookingService.SendQuery();
                var response = await pinger.SendQuery();
                return response;
            });
        }

        internal async Task<List<TData>> QueryData<TService, TData>(TService service) where TService : Requester<List<TData>>
        {
            return await _executionStrategy.Execute(async () =>
            {
                var data = await service.SendQuery();
                return data;
            });
        }

        /// <summary>
        /// The locales service can be used to return the list of localizations supported by Skyscanner.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Data.Locale>> QueryLocale()
        {
            return await QueryData<Locale, Data.Locale>(new Locale(_apiKey));
        }
        /// <summary>
        /// The location autosuggest service can be used to get a list of places (with corresponding IDs) that 
        /// match the query string.
        /// </summary>
        /// <param name="query">Query string to search for</param>
        /// <returns></returns>
        public async Task<List<Location>> QueryLocation(string query)
        {
            return await QueryData<LocationAutosuggest, Location>(new LocationAutosuggest(_apiKey, 
                new LocationAutosuggestSettings(query)));
        }
        /// <summary>
        /// The location autosuggest service can be used to get a list of places (with corresponding IDs) that 
        /// match the query string. Or to get information about a specific place given it's ID (for example 
        /// city name and country name for an airport)
        /// </summary>
        /// <param name="settings">Settings for the query</param>
        /// <returns></returns>
        public async Task<List<Location>> QueryLocation(LocationAutosuggestSettings settings)
        {
            return await QueryData<LocationAutosuggest, Location>(new LocationAutosuggest(_apiKey, settings));
        }
        /// <summary>
        /// The markets service can be used to return the list of markets (countries) supported by Skyscanner.
        /// </summary>
        /// <param name="locale">Selected language</param>
        /// <returns></returns>
        public async Task<List<Data.Market>> QueryMarket(Data.Locale locale)
        {
            return await QueryData<Market, Data.Market>(new Market(_apiKey, locale));
        }
        /// <summary>
        /// The currency service can be used to get the list of valid currencies supported by Skyscanner, 
        /// with additional information about how to display them.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Data.Currency>> QueryCurrency()
        {
            return await QueryData<Currency, Data.Currency>(new Currency(_apiKey));
        }
    }
}