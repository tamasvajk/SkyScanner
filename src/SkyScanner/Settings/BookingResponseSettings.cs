// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingResponseSettings : PingResponseSettings
    {
        public BookingResponseSettings(Itinerary itinerary, CarrierSchema carrierSchema = CarrierSchema.Iata, LocationSchema locationSchema = LocationSchema.Iata)
        {
            FlightResponse = itinerary.FlightResponse;
            CarrierSchema = carrierSchema;
            LocationSchema = locationSchema;
            Itinerary = itinerary;
        }

        public LocationSchema LocationSchema { get; }

        internal CarrierSchema CarrierSchema { get; }

        [JsonIgnore]
        internal FlightResponse FlightResponse { get; }

        [JsonIgnore]
        internal Itinerary Itinerary { get; }
    }
}