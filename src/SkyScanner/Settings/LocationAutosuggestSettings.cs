// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using SkyScanner.Data;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Settings for the location autosuggest query
    /// </summary>
    public class LocationAutosuggestSettings
    {
        /// <summary>
        /// Initializes a new instance of the LocationAutosuggestSettings with default values
        /// </summary>
        /// <param name="query">Query string to search for</param>
        public LocationAutosuggestSettings(string query)
            : this(query, LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, Locale.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LocationAutosuggestSettings with the specified parameters
        /// </summary>
        /// <param name="query">Query string to search for</param>
        /// <param name="queryType">Query type - search by name or ID</param>
        /// <param name="market">Market country</param>
        /// <param name="currency">Selected currency</param>
        /// <param name="locale">Selected language</param>
        public LocationAutosuggestSettings(string query, LocationAutosuggestQueryType queryType, Market market, Currency currency, Locale locale)
        {
            if (market == null)
            {
                throw new ArgumentNullException(nameof(market));
            }
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }
            if (locale == null)
            {
                throw new ArgumentNullException(nameof(locale));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (query.Length < 2)
            {
                throw new ArgumentException("The query string has to be at least two characters long", nameof(query));
            }

            Query = query;
            QueryType = queryType;
            Market = market;
            Currency = currency;
            Locale = locale;
        }
        public string Query { get; }

        public Market Market { get; }

        public Currency Currency { get; }

        public Locale Locale { get; }

        public LocationAutosuggestQueryType QueryType { get; }
    }
}