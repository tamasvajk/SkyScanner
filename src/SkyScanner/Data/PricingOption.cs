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
        /// <summary>
        /// In case the deeplink is not supplied, you can obtain them with a further step by querying the booking service.
        /// </summary>
        [JsonProperty("DeeplinkUrl")]
        public Uri DeepLink { get; internal set; }
        /// <summary>
        /// If you supplied groupPricing=true, the price is the total for all passengers. 
        /// Otherwise, the price is the total price for 1 adult.
        /// </summary>
        public decimal Price { get; internal set; }
        /// <summary>
        /// Quote age in minutes
        /// </summary>
        [JsonProperty("QuoteAgeInMinutes")]
        public int QuoteAge { get; internal set; }
        [JsonProperty("Agents")]
        internal List<int> AgentIds { get; set; }
        /// <summary>
        /// Full details of the PUT request to start polling booking details. This is not used by this library.
        /// For getting the actual booking details,  the session key of the flight query response is used.
        /// </summary>
        public BookingDetails BookingDetailsLink { get; internal set; }
        [JsonIgnore]
        internal FlightResponse FlightResponse { get; set; }
        [JsonIgnore]
        public List<Agent> Agents
        {
            get { return FlightResponse.Agents.Where(agent => AgentIds.Contains(agent.Id)).ToList(); }
        }

        public PricingOption()
        {
            AgentIds = new List<int>();
        }
    }
}