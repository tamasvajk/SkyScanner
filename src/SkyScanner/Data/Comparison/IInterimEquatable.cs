// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data.Comparison
{
    internal interface IInterimEquatable<in T>
    {
        bool ShallowEquals(T other);
        bool DeepEquals(T other);
        int GetShallowHashCode();
    }
}