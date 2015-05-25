// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingRequestSettings : RequestSettings
    {
        private readonly string _sessionKey;
        private readonly Itinerary _itinerary;

        public BookingRequestSettings(string sessionKey, Itinerary itinerary)
        {
            _sessionKey = sessionKey;
            _itinerary = itinerary;
        }

        [JsonIgnore]
        internal string SessionKey {
            get { return _sessionKey; }
        }
        [JsonIgnore]
        public Itinerary Itinerary {
            get { return _itinerary; }
        }

        internal string OutboundLegId
        {
            get { return OutboundLeg == null ? null : OutboundLeg.Id; }
        }
        internal string InboundLegId
        {
            get { return InboundLeg == null ? null : InboundLeg.Id; }
        }
        [JsonIgnore]
        public Leg OutboundLeg { get { return Itinerary.OutboundLeg; }}
        [JsonIgnore]
        public Leg InboundLeg { get { return Itinerary.InboundLeg; } }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
    }
}