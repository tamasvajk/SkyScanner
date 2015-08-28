// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Base;
using SkyScanner.Flight;

namespace SkyScanner.Data
{
    /// <summary>
    /// A Booking Item represents a single booking that has to be made.
    /// </summary>
    public class BookingItem
    {
        internal int AgentId { get; set; }
        /// <summary>
        /// The price in the currency specified in the query
        /// </summary>
        public decimal Price { get; internal set; }
        /// <summary>
        /// The price in the currency of the first segment. 
        /// This is only set if the currency of the country of origin is different from the currency specified in the query.
        /// </summary>
        public decimal AlternativePrice { get; internal set; }
        /// <summary>
        /// The currency of the first segment.
        /// This is only set if the currency of the country of origin is different from the currency specified in the query.
        /// </summary>
        public string AlternativeCurrency { get; internal set; }
        public Uri DeepLink { get; internal set; }
        /// <summary>
        /// The status of the price
        /// </summary>
        [JsonProperty("PriceStatus")]
        public BookingStatus PriceStatus { get; internal set; }
        internal List<int> SegmentIds { get; set; }

        /// <summary>
        /// The selling agent
        /// </summary>
        [JsonIgnore]
        public Agent Agent
        {
            get { return FlightResponse.Agents.FirstOrDefault(agent => AgentId == agent.Id); }
        }
        [JsonIgnore]
        internal FlightResponse FlightResponse { get; set; }
        [JsonIgnore]
        internal IContainerResponse ContainerResponse { get; set; }
        /// <summary>
        /// The details of each flight segment: Airports, times, carrier, duration, flight number, direction.
        /// </summary>
        [JsonIgnore]
        public List<LegSegment> Segments
        {
            get
            {
                return SegmentIds
                    .Select(id => ContainerResponse.Segments.SingleOrDefault(s => s.Id == id))
                    .ToList();
            }
        }
    }
}