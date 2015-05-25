// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NodaTime;
using SkyScanner.Data;
using SkyScanner.Settings.Base;

namespace SkyScanner.Settings
{
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
        private readonly CarrierSchema _carrierSchema;
        private readonly LocationSchema _locationSchema;

        public FlightRequestSettings(Location origin, Location destination, LocalDate outboundDate,
            LocalDate? inboundDate = null, int adults = 1, int children = 0, int infants = 0,
            bool groupPricing = true, CabinClass cabinClass = CabinClass.Economy,
            Market marketCountry = null, Currency currency = null, Locale locale = null,
            CarrierSchema carrierSchema = CarrierSchema.Iata, LocationSchema locationSchema = LocationSchema.Iata)
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
                throw new ArgumentException("Origin and destination are the same");
            }

            if (inboundDate.HasValue && inboundDate.Value < outboundDate)
            {
                throw new ArgumentException("Return flight cannot be earlier than the outbound flight");
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

            _carrierSchema = carrierSchema;
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