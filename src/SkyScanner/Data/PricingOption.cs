// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Flight;

namespace SkyScanner.Data
{
    public class PricingOption
    {
        [JsonProperty("DeeplinkUrl")]
        public Uri DeepLink { get; internal set; }
        public decimal Price { get; internal set; }
        [JsonProperty("QuoteAgeInMinutes")]
        public int QuoteAge { get; internal set; }
        [JsonProperty("Agents")]
        internal List<int> AgentIds { get; set; }
        public BookingDetails BookingDetailsLink { get; internal set; }
        [JsonIgnore]
        internal FlightResponse FlightResponse { get; set; }
        [JsonIgnore]
        public List<Agent> Agents
        {
            get { return FlightResponse.Agents.Where(agent => AgentIds.Contains(agent.Id)).ToList(); }
        }
    }
}