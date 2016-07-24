// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Default execution strategy, to simply execute a function
    /// </summary>
    public class DefaultExecutionStrategy : IExecutionStrategy
    {
        public async Task<T> Execute<T>(Func<Task<T>> func, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await func();
        }
    }
}