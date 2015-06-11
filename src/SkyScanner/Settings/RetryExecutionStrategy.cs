// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NodaTime;
using SkyScanner.Exceptions;
using SkyScanner.Services.Base;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Execution strategy with retry mechanism
    /// </summary>
    public class RetryExecutionStrategy : IExecutionStrategy
    {
        /// <summary>
        /// An initialized RetryExecutionStrategy with 1 second retry delay, and 2 retries on 
        /// unexpected exceptions
        /// </summary>
        public static readonly RetryExecutionStrategy Default = new RetryExecutionStrategy(Duration.FromSeconds(1));

        private readonly int _retryCountOnException;
        private readonly Duration _retryInterval;

        /// <summary>
        /// Initializes a new instance of the RetryExecutionStrategy with the specified parameters
        /// </summary>
        /// <param name="retryInterval">The duration to wait after a failed execution</param>
        /// <param name="retryCountOnException">The number of retries upon unexpected exceptions.</param>
        public RetryExecutionStrategy(Duration retryInterval, int retryCountOnException = 2)
        {
            _retryInterval = retryInterval;
            _retryCountOnException = retryCountOnException;
        }

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            return await Retry.Do<T, System.Exception>(func, _retryInterval, _retryCountOnException);
        }
    }
}