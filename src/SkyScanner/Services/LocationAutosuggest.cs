// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Services.Base;
using SkyScanner.Settings;

namespace SkyScanner.Services
{
    internal class LocationAutosuggest : Requester<List<Data.Location>>
    {
        private readonly LocationAutosuggestSettings _settings;
        
        public LocationAutosuggest(string apiKey, LocationAutosuggestSettings settings)
            : base(apiKey)
        {
            _settings = settings;
        }

        protected override Func<HttpClient, Task<HttpResponseMessage>> HttpMethod
        {
            get
            {
                return client => client.GetAsync(
                    string.Format(
                        "http://partners.api.skyscanner.net/apiservices/autosuggest/v1.0/{0}/{1}/{2}/?{5}={3}&apiKey={4}",
                        _settings.Market, _settings.Currency, _settings.Locale, _settings.Query, ApiKey, _settings.QueryType.ToString().ToLower()));
            }
        }

        protected override List<Location> CreateResponse(string content)
        {
            return JsonConvert.DeserializeObject<Response>(content, Scanner.JsonSerializerSettings).Places;
        }

        internal class Response
        {
            internal List<Location> Places { get; set; }
        }
    }
}