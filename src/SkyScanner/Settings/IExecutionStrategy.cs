// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace SkyScanner.Settings
{
    public interface IExecutionStrategy
    {
        Task<T> Execute<T>(Func<Task<T>> func);
    }
}