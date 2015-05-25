// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingResponseSettings : PingResponseSettings
    {
        private readonly FlightResponse _flightResponse;
        private readonly CarrierSchema _carrierSchema;
        private readonly LocationSchema _locationSchema;
        private readonly Itinerary _itinerary;
        
        public BookingResponseSettings(Itinerary itinerary, CarrierSchema carrierSchema = CarrierSchema.Iata, LocationSchema locationSchema = LocationSchema.Iata)
        {
            _flightResponse = itinerary.FlightResponse;
            _carrierSchema = carrierSchema;
            _locationSchema = locationSchema;
            _itinerary = itinerary;
        }

        public LocationSchema LocationSchema {
            get { return _locationSchema; }
        }
        internal CarrierSchema CarrierSchema {
            get { return _carrierSchema; }
        }

        [JsonIgnore]
        internal FlightResponse FlightResponse
        {
            get { return _flightResponse; }
        }

        [JsonIgnore]
        internal Itinerary Itinerary
        {
            get { return _itinerary; }
        }
    }
}