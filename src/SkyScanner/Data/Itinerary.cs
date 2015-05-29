// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SkyScanner.Data
{
    public class Itinerary
    {
        internal string OutboundLegId { get; set; }
        internal string InboundLegId { get; set; }
        /// <summary>
        /// Pricing options with agent(s), the quote age, price and deeplink URL (the absolute URL needed to make the booking).
        /// </summary>
        public List<PricingOption> PricingOptions { get; internal set; }
        private Flight.FlightResponse _flightResponse;
        [JsonIgnore]
        internal Flight.FlightResponse FlightResponse
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
    }
}