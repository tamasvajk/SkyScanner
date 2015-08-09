// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Comparison;
using SkyScanner.Flight;

namespace SkyScanner.Data
{
    public class Itinerary : IInterimEquatable<Itinerary>
    {
        public Itinerary()
        {
            PricingOptions = new List<PricingOption>();
        }

        internal string OutboundLegId { get; set; }
        internal string InboundLegId { get; set; }
        /// <summary>
        /// Pricing options with agent(s), the quote age, price and deeplink URL (the absolute URL needed to make the booking).
        /// </summary>
        public List<PricingOption> PricingOptions { get; internal set; }
        private FlightResponse _flightResponse;
        [JsonIgnore]
        internal FlightResponse FlightResponse
        {
            get { return _flightResponse; }
            set
            {
                PricingOptions.ForEach(option => option.FlightResponse = value);
                _flightResponse = value;
            }
        }
        [JsonIgnore]
        public Leg OutboundLeg
        {
            get { return FlightResponse.Legs.FirstOrDefault(leg => leg.Id == OutboundLegId); }
        }
        [JsonIgnore]
        public Leg InboundLeg
        {
            get { return FlightResponse.Legs.FirstOrDefault(leg => leg.Id == InboundLegId); }
        }
        
        bool IInterimEquatable<Itinerary>.ShallowEquals(Itinerary other)
        {
            return InboundLegId == other.InboundLegId && 
                OutboundLegId == other.OutboundLegId;
        }

        int IInterimEquatable<Itinerary>.GetShallowHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + InboundLegId.GetHashCode();
                hash = hash * 23 + OutboundLegId.GetHashCode();
                return hash;
            }
        }

        bool IInterimEquatable<Itinerary>.DeepEquals(Itinerary other)
        {
            return InboundLegId == other.InboundLegId && 
                OutboundLegId == other.OutboundLegId && 
                PricingOptions.Count == other.PricingOptions.Count;
        }
    }
}