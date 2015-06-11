// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using SkyScanner.Data;

namespace SkyScanner.Settings
{
    internal class BookingQuerySettings
    {
        public BookingQuerySettings(BookingRequestSettings requestSettings, Itinerary itinerary)
        {
            BookingRequest = requestSettings;
            BookingResponse = new BookingResponseSettings(itinerary);
        }

        public BookingRequestSettings BookingRequest { get; }

        public BookingResponseSettings BookingResponse { get; }
    }
}