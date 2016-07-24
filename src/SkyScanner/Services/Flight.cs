// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using SkyScanner.Flight;
using SkyScanner.Services.Base;
using SkyScanner.Settings;
using System.Threading;

namespace SkyScanner.Services
{
    internal class Flight : RequesterWithPing<FlightResponse>
    {
        private readonly FlightQuerySettings _querySettings;

        public Flight(string apiKey, FlightQuerySettings querySettings)
            : base(apiKey, querySettings.FlightRequest)
        {
            _querySettings = querySettings;
        }
        
        protected override Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> HttpMethod
        {
            get
            {
                return (client, token) =>
                    client.PostAsync("http://partners.api.skyscanner.net/apiservices/pricing/v1.0/",
                        GetFormContent(), token);
            }
        }
        
        protected override ResponsePinger<FlightResponse> CreatePinger(Uri sessionUri)
        {
            return new FlightResponsePinger(ApiKey, sessionUri, _querySettings.FlightResponse);
        }
    }
}
