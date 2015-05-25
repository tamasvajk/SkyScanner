// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Base;
using SkyScanner.Flight;

namespace SkyScanner.Data
{
    public class BookingItem
    {
        internal int AgentId { get; set; }
        public decimal Price { get; internal set; }
        public decimal AlternativePrice { get; internal set; }
        public string AlternativeCurrency { get; internal set; }
        public Uri DeepLink { get; internal set; }
        [JsonProperty("PriceStatus")]
        public BookingStatus PriceStatus { get; internal set; }
        internal List<int> SegmentIds { get; set; }

        [JsonIgnore]
        public Agent Agent
        {
            get { return FlightResponse.Agents.FirstOrDefault(agent => AgentId == agent.Id); }
        }
        [JsonIgnore]
        internal FlightResponse FlightResponse { get; set; }
        [JsonIgnore]
        internal IContainerResponse ContainerResponse { get; set; }
        [JsonIgnore]
        public List<LegSegment> Segments
        {
            get
            {
                return ContainerResponse.Segments.Where(segment => SegmentIds.Contains(segment.Id))
                    .OrderBy(segment => segment.DepartureTime)
                    .ToList();
            }
        }
    }
}