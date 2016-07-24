// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    internal class BookingRequestSettingsBase : RequestSettings
    {
        protected BookingRequestSettingsBase(string sessionKey)
        {
            SessionKey = sessionKey;
        }
        public BookingRequestSettingsBase(string sessionKey,
            string outboundLegId, string inboundLegId)
            : this(sessionKey)
        {
            OutboundLegId = outboundLegId;
            InboundLegId = inboundLegId;
        }

        [JsonIgnore]
        internal string SessionKey { get; }
        internal virtual string OutboundLegId { get; }
        internal virtual string InboundLegId { get; }

        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
    }
}