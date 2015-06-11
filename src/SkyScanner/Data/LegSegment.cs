// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class LegSegment
    {
        internal int Id { get; set; }
        internal int OriginStation { get; set; }
        internal int DestinationStation { get; set; }
        [JsonProperty("DepartureDateTime")]
        public NodaTime.LocalDateTime DepartureTime { get; internal set; }
        [JsonProperty("ArrivalDateTime")]
        public NodaTime.LocalDateTime ArrivalTime { get; internal set; }
        /// <summary>
        /// The duration in minutes
        /// </summary>
        public int Duration { get; internal set; }
        public JourneyMode JourneyMode { get; internal set; }
        public Directionality Directionality { get; internal set; }

        [JsonIgnore]
        public FlightInfo Flight => new FlightInfo
        {
            CarrierId = CarrierId,
            FlightNumber = FlightNumber,
            ContainerResponse = ContainerResponse
        };

        internal int FlightNumber { get; set; }
        [JsonProperty("Carrier")]
        internal int CarrierId { get; set; }
        [JsonIgnore]
        internal IContainerResponse ContainerResponse { get; set; }
        [JsonIgnore]
        public Place Origin
        {
            get { return ContainerResponse.Places.FirstOrDefault(place => place.Id == OriginStation); }
        }
        [JsonIgnore]
        public Place Destination
        {
            get { return ContainerResponse.Places.FirstOrDefault(place => place.Id == DestinationStation); }
        }
    }
}