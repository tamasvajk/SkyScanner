namespace SkyScanner.Services.Interfaces
{
    using SkyScanner.Data;
    using SkyScanner.Flight;

    /// <summary>
    /// The IInterimFlightResultHandler describes an interface for a handler that
    /// handles incoming interim responses from SkyScanner as a result of the polling
    /// process
    /// </summary>
    internal interface IInterimFlightResultHandler
    {
        /// <summary>
        /// Handles an (possibly) transforms incoming FlightResponse object according to
        /// the spec of the handler
        /// </summary>
        /// <param name="flightresponse">The flightresponse</param>
        /// <returns>The - possibly - transformed collection of itineraries</returns>
        InterimChangeSet<Itinerary> Handle(FlightResponse flightresponse);
    }
}