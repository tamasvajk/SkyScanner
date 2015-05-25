// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    // http://partners.api.skyscanner.net/apiservices/reference/v1.0/countries/en-GB?apiKey=prtl6749387986743898559646983194
    public class Market
    {
        public static readonly Market Default = new Market {Code = "UK", Name = "United Kingdom"};

        public string Code { get; internal set; }
        public string Name { get; internal set; }

        public override string ToString()
        { 
            return Code;
        }
    }
}