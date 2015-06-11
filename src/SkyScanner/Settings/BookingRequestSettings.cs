// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingRequestSettings : RequestSettings
    {
        public BookingRequestSettings(string sessionKey, Itinerary itinerary)
        {
            SessionKey = sessionKey;
            Itinerary = itinerary;
        }

        [JsonIgnore]
        internal string SessionKey { get; }

        [JsonIgnore]
        public Itinerary Itinerary { get; }

        internal string OutboundLegId => OutboundLeg?.Id;

        internal string InboundLegId => InboundLeg?.Id;

        [JsonIgnore]
        public Leg OutboundLeg => Itinerary.OutboundLeg;

        [JsonIgnore]
        public Leg InboundLeg => Itinerary.InboundLeg;

        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
    }
}