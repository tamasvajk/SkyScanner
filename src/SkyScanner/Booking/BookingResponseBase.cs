// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Data.Base;

namespace SkyScanner.Booking
{
    public class BookingResponseBase : IPingResponse, IContainerResponse
    {
        /// <summary>
        /// The list of booking options and items.
        /// A Booking Option is a complete option to book an itinerary. It is made up of Booking Items, where a
        /// Booking Item represents a single booking that has to be made. A multi-booking itinerary would have a
        /// single Booking Option which is made up of two or more items, with an item for each separate flight to
        /// be booked. A regular scheduled flight would have many Booking Options, each containing a single
        /// booking item. Each option would be for a different agent selling the same complete itinerary for a
        /// slightly different price.
        /// </summary>
        public List<BookingOption> BookingOptions { get; set; }

        internal List<LegSegment> Segments { get; set; }
        internal List<Carrier> Carriers { get; set; }
        internal List<Place> Places { get; set; }

        List<Place> IContainerResponse.Places => Places;

        List<LegSegment> IContainerResponse.Segments => Segments;

        List<Carrier> IContainerResponse.Carriers => Carriers;

        [JsonIgnore]
        bool IPingResponse.Succeeded
        {
            get
            {
                return BookingOptions
                    .All(bo => bo.BookingItems
                                    .All(item => item.PriceStatus != BookingStatus.Pending));
            }
        }
    }
}