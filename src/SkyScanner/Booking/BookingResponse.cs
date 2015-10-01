// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using SkyScanner.Data;

namespace SkyScanner.Booking
{
    public class BookingResponse : BookingResponseBase
    {
        internal BookingResponse(BookingResponseBase response)
        {
            BookingOptions = response.BookingOptions;
            Carriers = response.Carriers;
            Places = response.Places;
            Segments = response.Segments;
        }

        [JsonIgnore]
        public Itinerary Itinerary { get; internal set; }
    }
}