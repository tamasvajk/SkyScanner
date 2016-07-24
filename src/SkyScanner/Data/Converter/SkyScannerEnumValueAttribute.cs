// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Data.Converter
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class SkyScannerEnumValueAttribute : Attribute
    {
        public string Value { get; set; }
    }
}