// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SkyScanner.Data.Base;
using SkyScanner.Exceptions;
using SkyScanner.Settings.Base;

namespace SkyScanner.Services.Base
{
    internal abstract class ResponsePinger<TResponse> : HttpRetry<TResponse, RetryResponsePingException>
        where TResponse : class, IPingResponse
    {
        protected ResponsePinger(string apiKey) : base(apiKey, 1000)
        {
        }

        protected override async Task<TResponse> HandleResponse(HttpResponseMessage httpResponseMessage)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<TResponse>(content, Scanner.JsonSerializerSettings);

                    if (response.Succeeded)
                    {
                        PostProcess(response);
                        return response;
                    }

                    throw new RetryResponsePingException();
                case HttpStatusCode.NoContent:
                case HttpStatusCode.NotModified:
                    throw new RetryResponsePingException();
                default:
                    if ((int)httpResponseMessage.StatusCode == 429)
                    {
                        throw new RetryResponsePingException();
                    }
                    return await base.HandleResponse(httpResponseMessage);
            }
        }

        protected abstract void PostProcess(TResponse response);

        protected string GetQueryString(PingResponseSettings settings)
        {
            var settingsJson = JsonConvert.SerializeObject(settings, Scanner.JsonSerializerSettings);
            var dictSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsJson);
            dictSettings.Add("apiKey", ApiKey);
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var property in dictSettings)
            {
                query.Add(property.Key, property.Value);
            }

            return query.ToString();
        }
    }
}