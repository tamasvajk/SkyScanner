// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    /// <summary>
    /// Markets (countries) supported by Skyscanner
    /// </summary>
    public class Market
    {
        /// <summary>
        /// Default market - United Kingdom
        /// </summary>
        public static readonly Market Default = new Market {Code = "UK", Name = "United Kingdom"};

        /// <summary>
        /// Country code
        /// </summary>
        public string Code { get; internal set; }
        /// <summary>
        /// Country name in the requested language
        /// </summary>
        public string Name { get; internal set; }

        public override string ToString()
        { 
            return Code;
        }

        /// <summary>
        /// Instantiates a new Market for the provided market code
        /// </summary>
        /// <param name="code">The market code. E.g. UK</param>
        /// <returns>The market instance for the given code</returns>
        public static Market FromCode(string code)
        {
            return new Market() { Code = code };
        }
    }
}