// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SkyScanner.Exceptions;
using System.Threading;

namespace SkyScanner.Services.Base
{
    internal abstract class Requester<TResponse> : HttpRetry<TResponse, RetryRequestException>
        where TResponse : class
    {
        protected Requester(string apiKey)
            : base(apiKey)
        {
        }

        protected override async Task<TResponse> HandleResponse(HttpResponseMessage httpResponseMessage, 
            CancellationToken cancellationToken)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    return CreateResponse(content);
                default:
                    return await base.HandleResponse(httpResponseMessage, cancellationToken);
            }
        }
        protected abstract TResponse CreateResponse(string content);
    }
}