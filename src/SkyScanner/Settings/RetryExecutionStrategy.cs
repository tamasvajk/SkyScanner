// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NodaTime;
using SkyScanner.Exceptions;
using SkyScanner.Services.Base;

namespace SkyScanner.Settings
{
    /// <summary>
    /// todo add comments
    /// </summary>
    public class RetryExecutionStrategy : IExecutionStrategy
    {
        public static RetryExecutionStrategy Default = new RetryExecutionStrategy(Duration.FromSeconds(1));

        private readonly int _retryCountOnGenericException;
        private readonly int _retryCountOnExpectedException;
        private readonly Duration _retryInterval;

        public RetryExecutionStrategy(Duration retryInterval, int retryCountOnExpectedException = 1, int retryCountOnGenericException = 2)
        {
            _retryInterval = retryInterval;
            _retryCountOnExpectedException = retryCountOnExpectedException;
            _retryCountOnGenericException = retryCountOnGenericException;
        }

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            return await Retry.Do<T, RetryRequestException>(func, _retryInterval, _retryCountOnExpectedException, _retryCountOnGenericException);
        }
    }
}