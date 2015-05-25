// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Data
{
    public class BookingDetails
    {
        public Uri Uri { get; internal set; }
        public string Body { get; internal set; }
        public HttpVerbs Method { get; internal set; }
    }
}