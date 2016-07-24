// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyScanner.Services.Base;
using System.Threading;

namespace SkyScanner.Services
{
    internal class Market : Requester<List<Data.Market>>
    {
        private readonly Data.Locale _locale;
        public Market(string apiKey, Data.Locale locale)
            : base(apiKey)
        {
            _locale = locale;
        }

        protected override Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> HttpMethod
        {
            get
            {
                return (client, token) => client.GetAsync(
                    $"http://partners.api.skyscanner.net/apiservices/reference/v1.0/countries/{_locale}?apiKey={ApiKey}",
                    token);
            }
        }

        protected override List<Data.Market> CreateResponse(string content)
        {
            return JsonConvert.DeserializeObject<Response>(content, Scanner.JsonSerializerSettings).Countries;
        }

        internal class Response
        {
            internal List<Data.Market> Countries { get; set; }
        }
    }
}