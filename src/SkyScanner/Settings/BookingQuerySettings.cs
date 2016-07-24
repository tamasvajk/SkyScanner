// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using SkyScanner.Data;

namespace SkyScanner.Settings
{
    internal class BookingQuerySettings
    {
        public BookingQuerySettings(BookingRequestSettingsBase requestSettings,
            CarrierSchema carrierSchema = CarrierSchema.Iata,
            LocationSchema locationSchema = LocationSchema.Iata)
        {
            BookingRequest = requestSettings;
            BookingResponse = new BookingResponseSettingsBase(carrierSchema, locationSchema);
        }

        public BookingQuerySettings(BookingRequestSettingsBase requestSettings,
            Itinerary itinerary)
            : this(requestSettings)
        {
            BookingResponse = new BookingResponseSettings(itinerary);
        }

        public BookingRequestSettingsBase BookingRequest { get; }
        public BookingResponseSettingsBase BookingResponse { get; }
    }
}