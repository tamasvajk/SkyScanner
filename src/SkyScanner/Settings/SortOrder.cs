// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using SkyScanner.Data.Converter;

namespace SkyScanner.Settings
{
    public enum SortOrder
    {
        [SkyScannerEnumValue(Value = "asc")]
        Ascending,
        [SkyScannerEnumValue(Value = "desc")]
        Descending
    }
}