// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class Leg
    {
        internal string Id { get; set; }
        internal int OriginStation { get; set; }
        internal int DestinationStation { get; set; }
        [JsonProperty("Departure")]
        public NodaTime.LocalDateTime DepartureTime { get; internal set; }
        [JsonProperty("Arrival")]
        public NodaTime.LocalDateTime ArrivalTime { get; internal set; }
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
                return FlightResponse.Segments
                    .Where(segment => SegmentIds.Contains(segment.Id))
                    .OrderBy(segment => segment.DepartureTime)
                    .ToList();
            }
        }
        [JsonIgnore]
        public List<Carrier> Carriers
        {
            get
            {
                return FlightResponse.Carriers
                    .Where(carrier => CarrierIds.Contains(carrier.Id))
                    .ToList();
            }
        }
        [JsonIgnore]
        public List<Carrier> OperatingCarriers
        {
            get
            {
                return FlightResponse.Carriers
                    .Where(carrier => OperatingCarrierIds.Contains(carrier.Id))
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
            get { return FlightResponse.Places.Where(place => StopIds.Contains(place.Id)).ToList(); }
        }
    }
}