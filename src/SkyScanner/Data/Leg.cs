// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class Leg
    {
        internal string Id { get; set; }
        internal int OriginStation { get; set; }
        internal int DestinationStation { get; set; }
        [JsonProperty("Departure")]
        public LocalDateTime DepartureTime { get; internal set; }
        [JsonProperty("Arrival")]
        public LocalDateTime ArrivalTime { get; internal set; }
        public int Duration { get; internal set; }

        public JourneyMode JourneyMode { get; internal set; }
        public Directionality Directionality { get; internal set; }
        internal List<int> SegmentIds { get; set; }
        [JsonProperty("Carriers")]
        internal List<int> CarrierIds { get; set; }
        [JsonProperty("OperatingCarriers")]
        internal List<int> OperatingCarrierIds { get; set; }
        [JsonProperty("Stops")]
        internal List<int> StopIds { get; set; }
        [JsonProperty("FlightNumbers")]
        public List<FlightInfo> FlightInfos { get; internal set; }
        private IContainerResponse _flightResponse;
        [JsonIgnore]
        internal IContainerResponse FlightResponse
        {
            get { return _flightResponse; }
            set
            {
                FlightInfos.ForEach(info => info.ContainerResponse = value);
                _flightResponse = value;
            }
        }
        [JsonIgnore]
        public List<LegSegment> Segments
        {
            get
            {
                return SegmentIds
                    .Select(id => FlightResponse.Segments.SingleOrDefault(s => s.Id == id))
                    .ToList();
            }
        }
        [JsonIgnore]
        public List<Carrier> Carriers
        {
            get
            {
                return CarrierIds
                    .Select(id => FlightResponse.Carriers.SingleOrDefault(c => c.Id == id))
                    .ToList();
            }
        }
        [JsonIgnore]
        public List<Carrier> OperatingCarriers
        {
            get
            {
                return OperatingCarrierIds
                    .Select(id => FlightResponse.Carriers.SingleOrDefault(c => c.Id == id))
                    .ToList();
            }
        }
        [JsonIgnore]
        public Place Origin
        {
            get { return FlightResponse.Places.FirstOrDefault(place => place.Id == OriginStation); }
        }
        [JsonIgnore]
        public Place Destination
        {
            get { return FlightResponse.Places.FirstOrDefault(place => place.Id == DestinationStation); }
        }
        [JsonIgnore]
        public List<Place> Stops
        {
            get { return StopIds.Select(id => FlightResponse.Places.SingleOrDefault(pl => pl.Id == id)).ToList(); }
        }
    }
}