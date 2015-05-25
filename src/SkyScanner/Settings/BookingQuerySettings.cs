// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using SkyScanner.Data;

namespace SkyScanner.Settings
{
    internal class BookingQuerySettings
    {
        private readonly BookingRequestSettings _requestSettings;
        private readonly BookingResponseSettings _responseSettings;

        public BookingQuerySettings(BookingRequestSettings requestSettings, Itinerary itinerary)
        {
            _requestSettings = requestSettings;
            _responseSettings = new BookingResponseSettings(itinerary);
        }

        public BookingRequestSettings BookingRequest
        {
            get { return _requestSettings; }
        }

        public BookingResponseSettings BookingResponse
        {
            get { return _responseSettings; }
        }
    }
}