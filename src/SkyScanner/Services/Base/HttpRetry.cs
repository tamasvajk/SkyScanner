// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NodaTime;
using SkyScanner.Exceptions;
using SkyScanner.Services.Helpers;
using SkyScanner.Services.Interfaces;
using Exception = SkyScanner.Exceptions.Exception;

namespace SkyScanner.Services.Base
{
    internal abstract class HttpRetry<TResponse, TException> 
        where TException : Exception
    {
        private readonly int _initialDelay;

        protected readonly string ApiKey;
        protected abstract Func<HttpClient, Task<HttpResponseMessage>> HttpMethod { get; }

        private readonly ITaskDelayGenerator _delayGenerator = new IncreasingIntervalGenerator();

        protected HttpRetry(string apiKey, int initialDelay = 0)
        {
            _initialDelay = initialDelay;
            ApiKey = apiKey;
        }

        protected virtual Duration RetryTimeSpan => Duration.FromSeconds(1);

        public async Task<TResponse> SendQuery()
        {
            await Task.Delay(_initialDelay);

            return await Retry.Do<TResponse, TException>(
                async () =>
                    {
                        await Task.Delay(_delayGenerator.NextInterval);

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type","application/x-www-form-urlencoded");
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                            var httpResponseMessage = await HttpMethod(client);
                            return await HandleResponse(httpResponseMessage);
                        }
                    },
                RetryTimeSpan);
        }

        protected virtual Task<TResponse> HandleResponse(HttpResponseMessage httpResponseMessage)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException("Bad Request – Input validation failed");
                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException("Forbidden – The API Key was not supplied, or it was invalid, or it is not authorized to access the service.");
                case HttpStatusCode.InternalServerError:
                    throw new ServerErrorException("Server Error – An internal server error has occurred which has been logged.");
                default:
                    if ((int)httpResponseMessage.StatusCode == 429)
                    {
                        throw new RetryRequestException();
                    }
                    throw new NotSupportedException(
                        $"Status code {(int) httpResponseMessage.StatusCode} returned by the server is not supported");
            }
        }
    }
}