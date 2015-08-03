namespace SkyScanner.Services.Helpers
{
    using System.Diagnostics;
    using System.Linq;

    using SkyScanner.Data;
    using SkyScanner.Data.Comparison;
    using SkyScanner.Flight;
    using SkyScanner.Services.Interfaces;

    /// <summary>
    /// This type is an implementation of IInterimResultHandler that keeps track of what's been
    /// handled previously and it will use this state to compare new flightresponses and find
    /// only the new and updated itineraries.
    /// </summary>
    internal class DifferentialInterimFlightResultHandler : IInterimFlightResultHandler
    {
        /// <summary>
        /// The last response handled
        /// </summary>
        private FlightResponse lastResponse;

        public InterimChangeSet<Itinerary> Handle(FlightResponse flightresponse)
        {
            InterimChangeSet<Itinerary> result;
            if (this.lastResponse == null)
            {
                // In case tha lastresponse == null, in other words, this
                // is the first time Handle is called - the full response is returned
                result = new InterimChangeSet<Itinerary>(flightresponse.Itineraries, null, null);
                Debug.WriteLine($"Diff called for first time - with {flightresponse.Itineraries.Count()} itineraries");
            }
            else
            {
                // First: Add non-previously existing itineraries. Using shallow compare is enough to identify the missing
                // itineraries
                var additions = flightresponse.Itineraries
                        .Where(itinerary => !this.lastResponse.Itineraries.Contains(itinerary, new ShallowItineraryComparer()))
                        .ToList();

                // Secondly: use deep compare to find itineraries that were pre-existing, but for which the pricing options
                // have changed. The DeepItineraryComparer can identify these instances - although it will also find the 
                // new itineraries - so these must be excluded
                var updates = flightresponse.Itineraries
                        .Except(additions)
                        .Where(itinerary => !this.lastResponse.Itineraries.Contains(itinerary, new DeepItineraryComparer()))
                        .ToList();

                Debug.WriteLine(
                    $"Diff found {additions.Count()} new and {updates.Count()} updated itineraries - "
                    + $"now @ {flightresponse.Itineraries.Count()} itineraries");

                result = new InterimChangeSet<Itinerary>(flightresponse.Itineraries, additions, updates);
            }

            this.lastResponse = flightresponse;
            return result;
        }
    }
}