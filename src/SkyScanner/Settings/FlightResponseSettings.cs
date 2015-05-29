// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using SkyScanner.Data;
using SkyScanner.Data.Converter;
using System.Reflection;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Settings for the flight response query as specified by SkyScanner
    /// </summary>
    public class FlightResponseSettings : PingResponseSettings
    {
        private readonly int? _maxDuration;
        private readonly int? _maxStops;
        private readonly SortOrder _sortOrder;
        private readonly SortType _sortType;
        private readonly CarrierSchema _carrierSchema;
        private readonly LocationSchema _locationSchema;
        private readonly IEnumerable<string> _originAirports;
        private readonly IEnumerable<string> _destinationAirports;
        private readonly IEnumerable<string> _includeCarriers;
        private readonly IEnumerable<string> _excludeCarriers;
        private readonly DayTimePeriod? _outboundDepartureTime;
        private readonly LocalTime? _outboundDepartureStartTime;
        private readonly LocalTime? _outboundDepartureEndTime;
        private readonly LocalTime? _inboundDepartureEndTime;
        private readonly LocalTime? _inboundDepartureStartTime;
        private readonly DayTimePeriod? _inboundDepartureTime;

        /// <summary>
        /// Initializes a new instance of the FlightResponseSettings with the specified parameters
        /// </summary>
        /// <param name="sortType">The property to sort on</param>
        /// <param name="sortOrder">Sort direction</param>
        /// <param name="maxStops">Filter for maximum number of stops. Between 0 and 3</param>
        /// <param name="maxDuration">Filter for maximum duration in minutes</param>
        /// <param name="outboundDepartureTime">Filter for outbound departure time by time period of the day </param>
        /// <param name="outboundDepartureStartTime">Filter for start of range for outbound departure time</param>
        /// <param name="outboundDepartureEndTime">Filter for end of range for outbound departure time</param>
        /// <param name="inboundDepartureTime">Filter for inbound departure time by time period of the day </param>
        /// <param name="inboundDepartureStartTime">Filter for start of range for inbound departure time</param>
        /// <param name="inboundDepartureEndTime">Filter for end of range for inbound departure time</param>
        /// <param name="originAirports">Origin airports to filter on. List of airport codes</param>
        /// <param name="destinationAirports">Destination airports to filter on. List of airport codes</param>
        /// <param name="includeCarriers">Filter flights by the specified carriers. Must be Iata carrier codes</param>
        /// <param name="excludeCarriers">Filter flights by any but the specified carriers. Must be Iata carrier codes</param>
        /// <param name="carrierSchema">The code schema to use for carriers</param>
        /// <param name="locationSchema">The code schema used for locations</param>
        public FlightResponseSettings(
            SortType sortType = SortType.Price, SortOrder sortOrder = SortOrder.Ascending,
            int? maxStops = null, int? maxDuration = null,
            DayTimePeriod? outboundDepartureTime = null, LocalTime? outboundDepartureStartTime = null,
            LocalTime? outboundDepartureEndTime = null,
            DayTimePeriod? inboundDepartureTime = null, LocalTime? inboundDepartureStartTime = null,
            LocalTime? inboundDepartureEndTime = null,
            IEnumerable<string> originAirports = null, IEnumerable<string> destinationAirports = null,
            IEnumerable<string> includeCarriers = null, IEnumerable<string> excludeCarriers = null,
            CarrierSchema carrierSchema = CarrierSchema.Iata, LocationSchema locationSchema = LocationSchema.Iata)
        {
            if (maxStops.HasValue && (maxStops.Value < 0 || maxStops.Value > 3))
            {
                throw new ArgumentException("The filter for maximum number of stops must be between 0 and 3", "maxStops");
            }

            if (maxDuration.HasValue && (maxDuration.Value < 0 || maxDuration.Value > 1800))
            {
                throw new ArgumentException("The filter for maximum duration must be between 0 and 1800", "maxDuration");
            }

            _maxDuration = maxDuration;
            _maxStops = maxStops;

            _sortOrder = sortOrder;
            _sortType = sortType;

            _carrierSchema = carrierSchema;
            _locationSchema = locationSchema;

            _originAirports = originAirports;
            _destinationAirports = destinationAirports;
            _includeCarriers = includeCarriers;
            _excludeCarriers = excludeCarriers;

            _outboundDepartureTime = outboundDepartureTime;
            _outboundDepartureStartTime = outboundDepartureStartTime;
            _outboundDepartureEndTime = outboundDepartureEndTime;

            _inboundDepartureTime = inboundDepartureTime;
            _inboundDepartureStartTime = inboundDepartureStartTime;
            _inboundDepartureEndTime = inboundDepartureEndTime;
        }

        [JsonProperty("OriginAirports")]
        public string FormattedOriginAirports
        {
            get { return OriginAirports == null ? null : string.Join(";", OriginAirports); }
        }

        [JsonProperty("DestinationAirports")]
        public string FormattedDestinationAirports
        {
            get { return DestinationAirports == null ? null : string.Join(";", DestinationAirports); }
        }

        [JsonProperty("IncludeCarriers")]
        public string FormattedIncludeCarriers
        {
            get { return IncludeCarriers == null ? null : string.Join(";", IncludeCarriers); }
        }

        [JsonProperty("ExcludeCarriers")]
        public string FormattedExcludeCarriers
        {
            get { return ExcludeCarriers == null ? null : string.Join(";", ExcludeCarriers); }
        }

        [JsonIgnore]
        public IEnumerable<string> OriginAirports
        {
            get { return _originAirports; }
        }

        [JsonIgnore]
        public IEnumerable<string> DestinationAirports
        {
            get { return _destinationAirports; }
        }

        [JsonIgnore]
        public IEnumerable<string> IncludeCarriers
        {
            get { return _includeCarriers; }
        }

        [JsonIgnore]
        public IEnumerable<string> ExcludeCarriers
        {
            get { return _excludeCarriers; }
        }

        internal LocationSchema LocationSchema
        {
            get { return _locationSchema; }
        }

        internal CarrierSchema CarrierSchema
        {
            get { return _carrierSchema; }
        }

        public SortType SortType
        {
            get { return _sortType; }
        }

        [JsonConverter(typeof (SkyScannerStringEnumConverter))]
        public SortOrder SortOrder
        {
            get { return _sortOrder; }
        }

        [JsonProperty("Stops")]
        public int? MaxStops
        {
            get { return _maxStops; }
        }

        [JsonProperty("OutboundDepartTime")]
        internal string FormattedOutboundDepartureTime
        {
            get { return !OutboundDepartureTime.HasValue ? null : FormatDayTimePeriodList(OutboundDepartureTime.Value); }
        }

        [JsonIgnore]
        public DayTimePeriod? OutboundDepartureTime
        {
            get { return _outboundDepartureTime; }
        }

        [JsonProperty("OutboundDepartStartTime")]
        public LocalTime? OutboundDepartureStartTime
        {
            get { return _outboundDepartureStartTime; }
        }

        [JsonProperty("OutboundDepartEndTime")]
        public LocalTime? OutboundDepartureEndTime
        {
            get { return _outboundDepartureEndTime; }
        }

        [JsonProperty("InboundDepartTime")]
        internal string FormattedInboundDepartureTime
        {
            get { return !InboundDepartureTime.HasValue ? null : FormatDayTimePeriodList(InboundDepartureTime.Value); }
        }

        [JsonIgnore]
        public DayTimePeriod? InboundDepartureTime
        {
            get { return _inboundDepartureTime; }
        }

        [JsonProperty("InboundDepartStartTime")]
        public LocalTime? InboundDepartureStartTime
        {
            get { return _inboundDepartureStartTime; }
        }

        [JsonProperty("InboundDepartEndTime")]
        public LocalTime? InboundDepartureEndTime
        {
            get { return _inboundDepartureEndTime; }
        }

        /// <summary>
        /// In minutes
        /// </summary>
        [JsonProperty("Duration")]
        public int? MaxDuration
        {
            get { return _maxDuration; }
        }

        internal static string FormatDayTimePeriodList(DayTimePeriod self)
        {
            return string.Join(";",
                Enum.GetValues(typeof (DayTimePeriod))
                    .Cast<DayTimePeriod>()
                    .Where(p => self.HasFlag(p))
                    .Select(p => p.GetType().GetField(p.ToString())
                        .GetCustomAttributes<SkyScannerEnumValueAttribute>()
                        .First().Value));
        }
    }
}