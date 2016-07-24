// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingResponseSettings : BookingResponseSettingsBase
    {
        public BookingResponseSettings(Itinerary itinerary,
            CarrierSchema carrierSchema = CarrierSchema.Iata,
            LocationSchema locationSchema = LocationSchema.Iata)
            : base(carrierSchema, locationSchema)
        {
            FlightResponse = itinerary.FlightResponse;
            Itinerary = itinerary;
        }

        [JsonIgnore]
        internal FlightResponse FlightResponse { get; }

        [JsonIgnore]
        internal Itinerary Itinerary { get; }
    }
}