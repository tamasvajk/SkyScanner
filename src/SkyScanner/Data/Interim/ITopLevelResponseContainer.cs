using System.Collections.Generic;

namespace SkyScanner.Data.Interim
{
    internal interface ITopLevelResponseContainer<out T>
    {
        IReadOnlyList<T> TopLevelElements { get; }
    }
}