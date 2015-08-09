using System.Collections.Generic;

namespace SkyScanner.Data.Comparison
{
    internal class ShallowEqualityComparer<T> : IEqualityComparer<T>
        where T : IInterimEquatable<T>
    {
        public bool Equals(T x, T y)
        {
            return x.ShallowEquals(y);
        }
        
        public int GetHashCode(T obj)
        {
            return obj.GetShallowHashCode();
        }        
    }
}