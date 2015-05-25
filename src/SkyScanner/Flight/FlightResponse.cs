// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using SkyScanner.Data;
using SkyScanner.Data.Base;

namespace SkyScanner.Flight
{
    public class FlightResponse : IPingResponse, IContainerResponse
    {
        internal ResponseStatus Status { get; set; }
        internal string SessionKey { get; set; }
        public List<Itinerary> Itineraries { get; set; }
        internal List<Leg> Legs { get; set; }
        internal List<LegSegment> Segments { get; set; }
        internal List<Carrier> Carriers { get; set; }
        internal List<Agent> Agents { get; set; }
        internal List<Place> Places { get; set; }
        internal List<Currency> Currencies { get; set; }
        bool IPingResponse.Succeeded
        {
            get { return Status == ResponseStatus.UpdatesComplete; }
        }

        List<Place> IContainerResponse.Places
        {
            get { return Places; }
        }

        List<LegSegment> IContainerResponse.Segments
        {
            get { return Segments; }
        }
        
        List<Carrier> IContainerResponse.Carriers
        {
            get { return Carriers; }
        }
    }
}