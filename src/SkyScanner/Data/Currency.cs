// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SkyScanner.Data
{
    public class Currency
    {
        public static readonly Currency Default = new Currency
        {
            Code = "GBP",
            Symbol = "£",
            ThousandsSeparator = ",",
            DecimalSeparator = ".",
            SymbolOnLeft = true,
            SpaceBetweenAmountAndSymbol = false,
            RoundingCoefficient = 0,
            DecimalDigits = 2
        };
        
        public string Code { get; internal set; }
        public string Symbol { get; internal set; }
        internal string ThousandsSeparator { get; set; }
        internal string DecimalSeparator { get; set; }
        internal bool SymbolOnLeft { get; set; }
        internal bool SpaceBetweenAmountAndSymbol { get; set; }
        internal int RoundingCoefficient { get; set; }
        internal int DecimalDigits { get; set; }
        public override string ToString()
        {
            return Code;
        }
        public string FormatValue(decimal value)
        {
            var nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = DecimalSeparator,
                NumberGroupSeparator = ThousandsSeparator,
                NumberDecimalDigits = DecimalDigits
            };

            var v = value.ToString("N", nfi);
            var space = SpaceBetweenAmountAndSymbol ? " " : "";

            IEnumerable<string> parts = new[] { v, space, Symbol };

            if (SymbolOnLeft)
            {
                parts = parts.Reverse();
            }

            return string.Join("", parts);
        }
    }
}