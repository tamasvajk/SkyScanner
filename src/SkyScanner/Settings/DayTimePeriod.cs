// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using SkyScanner.Data.Converter;

namespace SkyScanner.Settings
{
    [Flags]
    public enum DayTimePeriod
    {
        [SkyScannerEnumValue(Value = "M")] Morning = 1,
        [SkyScannerEnumValue(Value = "A")] Afternoon = 2,
        [SkyScannerEnumValue(Value = "E")] Evening = 4
    }
}