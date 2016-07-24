// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingResponseSettingsBase : PingResponseSettings
    {
        public BookingResponseSettingsBase(
            CarrierSchema carrierSchema = CarrierSchema.Iata,
            LocationSchema locationSchema = LocationSchema.Iata)
        {
            CarrierSchema = carrierSchema;
            LocationSchema = locationSchema;
        }

        public LocationSchema LocationSchema { get; }

        internal CarrierSchema CarrierSchema { get; }
    }
}