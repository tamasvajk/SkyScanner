// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data;
using SkyScanner.Data.Base;

namespace SkyScanner.Booking
{
    public class BookingResponse : IPingResponse, IContainerResponse
    {
        [JsonIgnore]
        public Itinerary Itinerary { get; internal set; }

        public List<BookingOption> BookingOptions { get; set; }
        
        internal List<LegSegment> Segments { get; set; }
        internal List<Carrier> Carriers { get; set; }
        internal List<Place> Places { get; set; }
        
        List<Place> IContainerResponse.Places
        {
            get { return Places; }
        }

        List<LegSegment> IContainerResponse.Segments
        {
            get { return Segments; }
        }
        
        List<Carrier> IContainerResponse.Carriers
        {
            get { return Carriers; }
        }
        [JsonIgnore]
        bool IPingResponse.Succeeded
        {
            get { return BookingOptions.All(bo => bo.BookingItems.All(item => item.PriceStatus != BookingStatus.Pending)); }
        }
    }
}