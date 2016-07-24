using System.Diagnostics;
using System.Linq;
using SkyScanner.Data.Comparison;
using SkyScanner.Data.Interim;
using SkyScanner.Flight;
using SkyScanner.Services.Interfaces;

namespace SkyScanner.Services.Helpers
{
    using SkyScanner.Data.Base;

    /// <summary>
    /// This type is an implementation of IInterimResultHandler that keeps track of what's been
    /// handled previously and it will use this state to compare new flightresponses and find
    /// only the new and updated itineraries.
    /// </summary>
    internal class InterimResultProvider<TResponse, TResponseEntity> : IInterimResultProvider<TResponse, TResponseEntity>
        where TResponse : class, ITopLevelResponseContainer<TResponseEntity>, IPingResponse
        where TResponseEntity : class, IInterimEquatable<TResponseEntity>
    {
        /// <summary>
        /// The last response handled
        /// </summary>
        private TResponse _lastResponse;

        public InterimChangeSet<TResponseEntity> Calculate(TResponse response)
        {
            var newTopLevelElements = response.TopLevelElements;

            InterimChangeSet<TResponseEntity> result;
            if (_lastResponse == null)
            {
                // In case the lastresponse == null, in other words, this
                // is the first time Handle is called - the full response is returned
                result = new InterimChangeSet<TResponseEntity>(newTopLevelElements, null, null, response.Succeeded);
                Debug.WriteLine($"Diff called for first time - with {newTopLevelElements.Count} top level elements");
            }
            else
            {
                var previousTopLevelElements = _lastResponse.TopLevelElements;

                var unchanged = newTopLevelElements
                    .Select(element => new InterimPair<TResponseEntity>
                        {
                            New = element,
                            Original = previousTopLevelElements.FirstOrDefault(element.DeepEquals)
                        })
                    .Where(pair => pair.Original != null)
                    .ToList();

                var unchangedNewItems = unchanged.Select(element => element.New).ToList();

                var additions = newTopLevelElements
                    .Except(unchangedNewItems)
                    .Where(element => !previousTopLevelElements
                        .Contains(element, new ShallowEqualityComparer<TResponseEntity>()))
                    .ToList();

                var updates = newTopLevelElements
                    .Except(unchangedNewItems)
                    .Except(additions)
                    .Select(element => new InterimPair<TResponseEntity>
                        {
                            New = element,
                            Original = previousTopLevelElements.FirstOrDefault(element.ShallowEquals)
                        })
                    .Where(pair => pair.Original != null)
                    .ToList();

                Debug.WriteLine(
                    $"Diff found {additions.Count} new and {updates.Count} updated top level elements - "
                    + $"now at {newTopLevelElements.Count} top level elements");

                var mergedTopLevelElements = newTopLevelElements
                    .Except(unchangedNewItems)
                    .ToList();

                mergedTopLevelElements.AddRange(unchanged.Select(element => element.Original));

                result = new InterimChangeSet<TResponseEntity>(mergedTopLevelElements, additions, updates, response.Succeeded);
            }

            _lastResponse = response;
            return result;
        }
    }
}