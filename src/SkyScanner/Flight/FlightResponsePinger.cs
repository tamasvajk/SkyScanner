// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using SkyScanner.Services.Base;
using SkyScanner.Settings;
using System.Threading;

namespace SkyScanner.Flight
{
    internal class FlightResponsePinger : ResponsePinger<FlightResponse>
    {
        private readonly Uri _location;
        private readonly string _querySettings;

        public FlightResponsePinger(string apiKey, Uri sessionUri, FlightResponseSettings flightResponseSettings)
            : base(apiKey)
        {
            _location = sessionUri;
            _querySettings = GetQueryString(flightResponseSettings);
        }

        protected override Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> HttpMethod
        {
            get { return (client, token) => client.GetAsync($"{_location.AbsoluteUri}?{_querySettings}", token); }
        }

        protected override FlightResponse PostProcess(FlightResponse response, string rawContent)
        {
            response.Itineraries.ForEach(itinerary => { itinerary.FlightResponse = response; });
            response.Legs.ForEach(leg => { leg.FlightResponse = response; });
            response.Segments.ForEach(segment => { segment.ContainerResponse = response; });
            response.Places.ForEach(place => { place.ContainerResponse = response; });
            return response;
        }
    }
}