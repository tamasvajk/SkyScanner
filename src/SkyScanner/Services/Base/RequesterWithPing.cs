// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyScanner.Data.Base;
using SkyScanner.Exceptions;
using SkyScanner.Settings.Base;
using System.Threading;

namespace SkyScanner.Services.Base
{
    internal abstract class RequesterWithPing<TResponse> : HttpRetry<ResponsePinger<TResponse>, RetryRequestException> 
        where TResponse : class, IPingResponse
    {
        private readonly Dictionary<string, string> _queryRequestSettings;

        protected RequesterWithPing(string apiKey, RequestSettings querySettings)
            : base(apiKey)
        {
            var settingsJson = JsonConvert.SerializeObject(querySettings, Scanner.JsonSerializerSettings);
            _queryRequestSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsJson);
            _queryRequestSettings.Add("apiKey", ApiKey);
        }

        protected override async Task<ResponsePinger<TResponse>> HandleResponse(
            HttpResponseMessage httpResponseMessage,
            CancellationToken cancellationToken)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.Created:
                    return await Task.FromResult(CreatePinger(httpResponseMessage.Headers.Location));
                case HttpStatusCode.Gone:
                case HttpStatusCode.NoContent:
                    throw new RetryRequestException();
                default:
                    return await base.HandleResponse(httpResponseMessage, cancellationToken);
            }
        }

        protected abstract ResponsePinger<TResponse> CreatePinger(Uri sessionUri);

        protected FormUrlEncodedContent GetFormContent()
        {
            return new FormUrlEncodedContent(_queryRequestSettings);
        }
    }
}