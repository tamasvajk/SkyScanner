// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Data
{
    public class Carrier
    {
        internal int Id { get; set; }
        public string Code { get; internal set; }
        public string DisplayCode { get; internal set; }
        public string Name { get; internal set; }
        public Uri ImageUrl { get; internal set; }
    }
}