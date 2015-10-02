// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyScanner.Services.Base;
using System.Threading;

namespace SkyScanner.Services
{
    internal class Currency : Requester<List<Data.Currency>>
    {
        public Currency(string apiKey)
            : base(apiKey)
        {
        }
        protected override Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> HttpMethod
        {
            get
            {
                return (client, token) => client.GetAsync(
                    $"http://partners.api.skyscanner.net/apiservices/reference/v1.0/currencies?apiKey={ApiKey}", token);
            }
        }

        protected override List<Data.Currency> CreateResponse(string content)
        {
            return JsonConvert.DeserializeObject<Response>(content, Scanner.JsonSerializerSettings).Currencies;
        }

        internal class Response
        {
            internal List<Data.Currency> Currencies { get; set; }
        }
    }
}