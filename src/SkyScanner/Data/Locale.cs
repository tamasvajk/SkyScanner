// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    public class Locale
    {
        public static readonly Locale Default = new Locale {Code = "en-GB", Name = "English"};
        
        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public override string ToString()
        {
            return Code;
        }
    }
}