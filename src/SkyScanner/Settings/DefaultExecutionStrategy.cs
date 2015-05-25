// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace SkyScanner.Settings
{
    public class DefaultExecutionStrategy : IExecutionStrategy
    {
        //public async Task<T> Execute<T, TException>(Func<Task<T>> func) where TException : Exceptions.Exception
        //{
        //    return await func();
        //}

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            return await func();
        }
    }
}