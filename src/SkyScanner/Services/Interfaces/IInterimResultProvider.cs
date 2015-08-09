using SkyScanner.Data.Comparison;
using SkyScanner.Data.Interim;
using SkyScanner.Flight;

namespace SkyScanner.Services.Interfaces
{
    /// <summary>
    /// The IInterimFlightResultHandler describes an interface for a handler that
    /// handles incoming interim responses from SkyScanner as a result of the polling
    /// process
    /// </summary>
    internal interface IInterimResultProvider<in TResponse, TResponseEntity>
        where TResponseEntity : IInterimEquatable<TResponseEntity>
        where TResponse : ITopLevelResponseContainer<TResponseEntity>
    {
        InterimChangeSet<TResponseEntity> Calculate(TResponse flightresponse);
    }
}