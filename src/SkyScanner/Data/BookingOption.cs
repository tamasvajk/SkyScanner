// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace SkyScanner.Data
{
    /// <summary>
    /// A Booking Option is a complete option to book an itinerary. It is made up of Booking Items
    /// </summary>
    public class BookingOption
    {
        public List<BookingItem> BookingItems { get; internal set; }
    }
}