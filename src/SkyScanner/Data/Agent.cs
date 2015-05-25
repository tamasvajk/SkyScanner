// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class Agent
    {
        internal int Id { get; set; }
        public string Name { get; internal set; }
        public Uri ImageUrl { get; internal set; }
        internal ResponseStatus Status { get; set; }
        public bool OptimizedForMobile { get; internal set; }
        [JsonProperty("BookingNumber")]
        public string BookingPhoneNumber { get; internal set; }
        public AgentType Type { get; internal set; }
    }
}