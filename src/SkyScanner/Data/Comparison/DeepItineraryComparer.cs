namespace SkyScanner.Data.Comparison
{
    using System.Collections.Generic;

    /// <summary>
    /// The deep Itinerary comparer. Compares two instance of Itinerary and will 
    /// take the number of pricing options into account to determine whether
    /// the instances are equal.
    /// 
    /// This assumes that the content of a pricing option does not change once it's set
    /// and that only additional options will be added over time by SkyScanner 
    /// as the polling progresses.
    /// </summary>
    public class DeepItineraryComparer : IEqualityComparer<Itinerary>
    {
        public bool Equals(Itinerary x, Itinerary y)
        {
            return x.InboundLegId == y.InboundLegId && x.OutboundLegId == y.OutboundLegId
                   && x.PricingOptions.Count == y.PricingOptions.Count;
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