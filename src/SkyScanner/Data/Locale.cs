// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    /// <summary>
    /// Localization supported by Skyscanner
    /// </summary>
    public class Locale
    {
        /// <summary>
        /// Default locale - British English
        /// </summary>
        public static readonly Locale Default = new Locale {Code = "en-GB", Name = "English"};
        
        /// <summary>
        /// Code of the locale. Two-letter lowercase culture code associated with a language and 
        /// a two-letter uppercase subculture code associated with a country or region.
        /// </summary>
        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public override string ToString()
        {
            return Code;
        }
    }
}