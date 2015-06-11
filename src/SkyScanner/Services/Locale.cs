// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyScanner.Services.Base;

namespace SkyScanner.Services
{
    internal class Locale : Requester<List<Data.Locale>>
    {
        public Locale(string apiKey)
            : base(apiKey)
        {
        }
        protected override Func<HttpClient, Task<HttpResponseMessage>> HttpMethod
        {
            get
            {
                return client => client.GetAsync(
                    $"http://partners.api.skyscanner.net/apiservices/reference/v1.0/locales?apiKey={ApiKey}");
            }
        }

        protected override List<Data.Locale> CreateResponse(string content)
        {
            return JsonConvert.DeserializeObject<Response>(content, Scanner.JsonSerializerSettings).Locales;
        }

        internal class Response
        {
            internal List<Data.Locale> Locales { get; set; }
        }
    }
}