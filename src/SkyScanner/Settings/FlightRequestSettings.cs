// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NodaTime;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Settings for the flight request query as specified by SkyScanner
    /// </summary>
    public class FlightRequestSettings : RequestSettings
    {
        /// <summary>
        /// Initializes a new instance of the FlightRequestSettings with the specified parameters
        /// </summary>
        /// <param name="origin">The origin city or airport</param>
        /// <param name="destination">The destination city or airport</param>
        /// <param name="outboundDate">The departure date</param>
        /// <param name="inboundDate">The return date if the query is for a two-way flight</param>
        /// <param name="adults">Number of adults traveling (min 0, max 8)</param>
        /// <param name="children">Number of children traveling (min 0, max 8)</param>
        /// <param name="infants">Number of infants traveling (min 0, max number of adults)</param>
        /// <param name="groupPricing">Show price-per-adult (false), or price for all passengers (true)</param>
        /// <param name="cabinClass">Cabin class of the flight</param>
        /// <param name="marketCountry">The user’s market country</param>
        /// <param name="currency">The user’s currency</param>
        /// <param name="locale">The user’s localization preference</param>
        /// <param name="locationSchema">The code schema used for locations</param>
        public FlightRequestSettings(Location origin, Location destination, LocalDate outboundDate,
            LocalDate? inboundDate = null, int adults = 1, int children = 0, int infants = 0,
            bool groupPricing = true, CabinClass cabinClass = CabinClass.Economy,
            Market marketCountry = null, Currency currency = null, Locale locale = null,
            LocationSchema locationSchema = LocationSchema.Iata)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (origin.PlaceId == destination.PlaceId)
            {
                throw new ArgumentException("Origin and destination are the same", nameof(destination));
            }

            if (inboundDate.HasValue && inboundDate.Value < outboundDate)
            {
                throw new ArgumentException("Return flight cannot be earlier than the outbound flight", nameof(outboundDate));
            }

            if (adults < 0 || adults > 8)
            {
                throw new ArgumentException("The number of adults traveling must be between 0 and 8", nameof(adults));
            }

            if (children < 0 || children > 8)
            {
                throw new ArgumentException("The number of children traveling must be between 0 and 8", nameof(children));
            }

            if (infants < 0 || infants > adults)
            {
                throw new ArgumentException("The number of infants traveling must be between 0 and the number of adults", nameof(infants));
            }

            if (adults == 0 && children == 0)
            {
                throw new ArgumentException("Can't search for 0 person", nameof(adults));

            }

            Origin = origin;
            Destination = destination;
            OutboundDate = outboundDate;

            InboundDate = inboundDate;

            Adults = adults;
            Children = children;
            Infants = infants;

            GroupPricing = groupPricing;
            CabinClass = cabinClass;

            MarketCountry = marketCountry ?? Market.Default;
            Currency = currency ?? Currency.Default;
            Locale = locale ?? Locale.Default;

            LocationSchema = locationSchema;
        }

        internal string Country => MarketCountry.ToString();

        [JsonIgnore]
        public Market MarketCountry { get; }

        [JsonProperty("Currency")]
        internal string CurrencyText => Currency.ToString();

        [JsonIgnore]
        public Currency Currency { get; }

        [JsonProperty("Locale")]
        internal string LocaleText => Locale.ToString();

        [JsonIgnore]
        public Locale Locale { get; }

        internal LocationSchema LocationSchema { get; }

        internal string OriginPlace => Origin.ToString();

        [JsonIgnore]
        public Location Origin { get; }

        internal string DestinationPlace => Destination.ToString();

        [JsonIgnore]
        public Location Destination { get; }

        public LocalDate OutboundDate { get; }

        public LocalDate? InboundDate { get; }

        public CabinClass CabinClass { get; }

        public int Adults { get; }

        public int Children { get; }

        public int Infants { get; }

        public bool GroupPricing { get; }
    }
}