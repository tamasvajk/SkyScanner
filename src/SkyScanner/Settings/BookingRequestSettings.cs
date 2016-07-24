// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingRequestSettings : BookingRequestSettingsBase
    {
        public BookingRequestSettings(string sessionKey, Itinerary itinerary)
            : base(sessionKey)
        {
            Itinerary = itinerary;
        }

        [JsonIgnore]
        public Itinerary Itinerary { get; }

        internal override string OutboundLegId => OutboundLeg?.Id;

        internal override string InboundLegId => InboundLeg?.Id;

        [JsonIgnore]
        public Leg OutboundLeg => Itinerary.OutboundLeg;

        [JsonIgnore]
        public Leg InboundLeg => Itinerary.InboundLeg;
    }
}