// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using SkyScanner.Services.Base;
using SkyScanner.Settings;
using System.Threading;

namespace SkyScanner.Booking
{
    internal class BookingResponsePinger : ResponsePinger<BookingResponseBase>
    {
        private readonly Uri _location;
        private readonly BookingResponseSettingsBase _bookingResponseSettings;
        private readonly string _querySettings;

        public BookingResponsePinger(string apiKey, Uri sessionUri,
            BookingResponseSettingsBase bookingResponseSettings)
            : base(apiKey)
        {
            _location = sessionUri;
            _bookingResponseSettings = bookingResponseSettings;
            _querySettings = GetQueryString(bookingResponseSettings);
        }

        protected override Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> HttpMethod
        {
            get { return (client, token) => client.GetAsync($"{_location.AbsoluteUri}?{_querySettings}", token); }
        }

        protected override BookingResponseBase PostProcess(BookingResponseBase response, string rawContent)
        {
            var bookingResponseSettings = _bookingResponseSettings as BookingResponseSettings;
            var newResponse = bookingResponseSettings != null
                ? new BookingResponse(response)
                {
                    Itinerary = bookingResponseSettings.Itinerary
                }
                : response;

            response.Segments.ForEach(segment => { segment.ContainerResponse = newResponse; });
            response.Places.ForEach(place => { place.ContainerResponse = newResponse; });

            response.BookingOptions.ForEach(option => option.BookingItems.ForEach(item =>
            {
                item.ContainerResponse = newResponse;
            }));

            return newResponse;
        }
    }
}