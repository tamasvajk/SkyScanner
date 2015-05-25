// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using SkyScanner.Data;

namespace SkyScanner.Settings
{
    public class LocationAutosuggestSettings
    {
        private readonly Market _market;
        private readonly Currency _currency;
        private readonly Locale _locale;
        private readonly LocationAutosuggestQueryType _queryType;

        public LocationAutosuggestSettings()
            : this(LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, Locale.Default)
        {
        }

        public LocationAutosuggestSettings(LocationAutosuggestQueryType queryType, Market market, Currency currency, Locale locale)
        {
            if (market == null)
            {
                throw new ArgumentNullException("market");
            }
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }
            if (locale == null)
            {
                throw new ArgumentNullException("locale");
            }

            _queryType = queryType;
            _market = market;
            _currency = currency;
            _locale = locale;
        }
        public Market Market {
            get { return _market; }
        }
        public Currency Currency {
            get { return _currency; }
        }
        public Locale Locale {
            get { return _locale; }
        }
        public LocationAutosuggestQueryType QueryType
        {
            get { return _queryType; }
        }
    }
}