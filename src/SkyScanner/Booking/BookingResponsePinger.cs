// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using SkyScanner.Services.Base;
using SkyScanner.Settings;

namespace SkyScanner.Booking
{
    internal class BookingResponsePinger : ResponsePinger<BookingResponse>
    {
        private readonly Uri _location;
        private readonly BookingResponseSettings _bookingResponseSettings;
        private readonly string _querySettings;
        public BookingResponsePinger(string apiKey, Uri sessionUri, BookingResponseSettings bookingResponseSettings)
            :base(apiKey)
        {
            _location = sessionUri;
            _bookingResponseSettings = bookingResponseSettings;
            _querySettings = GetQueryString(bookingResponseSettings);
        }
        
        protected override Func<HttpClient, Task<HttpResponseMessage>> HttpMethod
        {
            get { return client => client.GetAsync($"{_location.AbsoluteUri}?{_querySettings}"); }
        }
        
        protected override void PostProcess(BookingResponse response)
        {
            response.Segments.ForEach(segment => { segment.ContainerResponse = response; });
            response.Places.ForEach(place => { place.ContainerResponse = response; });
            response.BookingOptions.ForEach(option => option.BookingItems.ForEach(item =>
            {
                item.ContainerResponse = response;
                item.FlightResponse = _bookingResponseSettings.FlightResponse;
            }));
            response.Itinerary = _bookingResponseSettings.Itinerary;
        }
    }
}