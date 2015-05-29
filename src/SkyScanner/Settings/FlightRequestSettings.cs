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
        private readonly Locale _locale;
        private readonly Currency _currency;
        private readonly Market _marketCountry;
        private readonly CabinClass _cabinClass;
        private readonly bool _groupPricing;
        private readonly int _infants;
        private readonly int _children;
        private readonly int _adults;
        private readonly LocalDate? _inboundDate;
        private readonly LocalDate _outboundDate;
        private readonly Location _destination;
        private readonly Location _origin;
        private readonly LocationSchema _locationSchema;

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
                throw new ArgumentNullException("origin");
            }
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            if (origin.PlaceId == destination.PlaceId)
            {
                throw new ArgumentException("Origin and destination are the same", "destination");
            }

            if (inboundDate.HasValue && inboundDate.Value < outboundDate)
            {
                throw new ArgumentException("Return flight cannot be earlier than the outbound flight", "outboundDate");
            }

            if (adults < 0 || adults > 8)
            {
                throw new ArgumentException("The number of adults traveling must be between 0 and 8", "adults");
            }

            if (children < 0 || children > 8)
            {
                throw new ArgumentException("The number of children traveling must be between 0 and 8", "children");
            }

            if (infants < 0 || infants > adults)
            {
                throw new ArgumentException("The number of infants traveling must be between 0 and the number of adults", "infants");
            }

            if (adults == 0 && children == 0)
            {
                throw new ArgumentException("Can't search for 0 person", "adults");

            }

            _origin = origin;
            _destination = destination;
            _outboundDate = outboundDate;

            _inboundDate = inboundDate;

            _adults = adults;
            _children = children;
            _infants = infants;

            _groupPricing = groupPricing;
            _cabinClass = cabinClass;

            _marketCountry = marketCountry ?? Market.Default;
            _currency = currency ?? Currency.Default;
            _locale = locale ?? Locale.Default;

            _locationSchema = locationSchema;
        }

        internal string Country
        {
            get { return MarketCountry.ToString(); }
        }

        [JsonIgnore]
        public Market MarketCountry
        {
            get { return _marketCountry; }
        }

        [JsonProperty("Currency")]
        internal string CurrencyText
        {
            get { return Currency.ToString(); }
        }

        [JsonIgnore]
        public Currency Currency
        {
            get { return _currency; }
        }

        [JsonProperty("Locale")]
        internal string LocaleText
        {
            get { return Locale.ToString(); }
        }

        [JsonIgnore]
        public Locale Locale
        {
            get { return _locale; }
        }

        internal LocationSchema LocationSchema
        {
            get { return _locationSchema; }
        }

        internal string OriginPlace
        {
            get { return Origin.ToString(); }
        }

        [JsonIgnore]
        public Location Origin
        {
            get { return _origin; }
        }

        internal string DestinationPlace
        {
            get { return Destination.ToString(); }
        }

        [JsonIgnore]
        public Location Destination
        {
            get { return _destination; }
        }

        public LocalDate OutboundDate
        {
            get { return _outboundDate; }
        }

        public LocalDate? InboundDate
        {
            get { return _inboundDate; }
        }

        public CabinClass CabinClass
        {
            get { return _cabinClass; }
        }

        public int Adults
        {
            get { return _adults; }
        }

        public int Children
        {
            get { return _children; }
        }

        public int Infants
        {
            get { return _infants; }
        }

        public bool GroupPricing
        {
            get { return _groupPricing; }
        }
    }
}