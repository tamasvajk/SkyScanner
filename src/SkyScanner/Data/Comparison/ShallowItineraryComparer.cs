namespace SkyScanner.Data.Comparison
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The shallow Itinerary comparer. Shallow compares two itineraries by checking whether
    /// their inbound and outbound leg ids are equal.
    /// </summary>
    public class ShallowItineraryComparer : IEqualityComparer<Itinerary>
    {
        public bool Equals(Itinerary x, Itinerary y)
        {
            return x.InboundLegId == y.InboundLegId && x.OutboundLegId == y.OutboundLegId;
        }

        /// <summary>
        /// Taken from http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        /// </summary>
        /// <param name="obj">The instance</param>
        /// <returns>The hash code for the instance</returns>
        public int GetHashCode(Itinerary obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + obj.InboundLegId.GetHashCode();
                hash = hash * 23 + obj.OutboundLegId.GetHashCode();
                hash = hash * 23 + obj.PricingOptions.GetHashCode();
                return hash;
            }
        }
    }
}