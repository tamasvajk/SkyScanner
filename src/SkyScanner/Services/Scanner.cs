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
using SkyScanner.Flight;
using SkyScanner.Services.Base;
using SkyScanner.Services.Helpers;
using SkyScanner.Settings;

namespace SkyScanner.Services
{
    public sealed class Scanner
    {
        private readonly string _apiKey;
        private readonly IExecutionStrategy _executionStrategy;

        public Scanner(string apiKey)
            :this (apiKey, new DefaultExecutionStrategy())
        {
        }

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

        public async Task<FlightResponse> QueryFlight(FlightQuerySettings flightQuerySettings)
        {
            var flightService = new Flight(_apiKey, flightQuerySettings);
            return await _executionStrategy.Execute(async () =>
            {
                var pinger = await flightService.SendQuery();
                var response = await pinger.SendQuery();
                return response;
            });
        }

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

        public async Task<List<Data.Locale>> QueryLocale()
        {
            return await QueryData<Locale, Data.Locale>(new Locale(_apiKey));
        }
        public async Task<List<Location>> QueryLocation(string query)
        {
            return await QueryData<LocationAutosuggest, Location>(new LocationAutosuggest(_apiKey, query,
                new LocationAutosuggestSettings()));
        }
        public async Task<List<Location>> QueryLocation(string query, LocationAutosuggestSettings settings)
        {
            return await QueryData<LocationAutosuggest, Location>(new LocationAutosuggest(_apiKey, query, settings));
        }
        public async Task<List<Data.Market>> QueryMarket(Data.Locale locale)
        {
            return await QueryData<Market, Data.Market>(new Market(_apiKey, locale));
        }
        public async Task<List<Data.Currency>> QueryCurrency()
        {
            return await QueryData<Currency, Data.Currency>(new Currency(_apiKey));
        }
    }
}