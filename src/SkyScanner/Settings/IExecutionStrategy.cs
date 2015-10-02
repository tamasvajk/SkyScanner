// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Provides a strategy to execute a given function
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Performs the given function according to a custom logic
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="func">The function to execute</param>
        /// <returns></returns>
        Task<T> Execute<T>(Func<Task<T>> func,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}