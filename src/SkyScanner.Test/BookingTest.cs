// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using SkyScanner.Settings;

namespace SkyScanner.Test
{
    [TestClass]
    public class BookingTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Bookings_Can_Be_Queried()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Data.Location.FromString("MAD-sky"),
                    Data.Location.FromString("WASA-sky"),
                    departureDate, departureDate.PlusDays(5)),
                new FlightResponseSettings()));

            var itinerary = itineraries.First();

            var bookingResponse = await Scanner.QueryBooking(itinerary);

            Assert.IsNotNull(bookingResponse);
            Assert.AreNotEqual(0, bookingResponse.BookingOptions.Count);
            var firstBookingItem = bookingResponse.BookingOptions.First().BookingItems.First();

            Assert.IsNotNull(firstBookingItem.Agent);
            Assert.AreNotEqual(0, firstBookingItem.Segments.Count);

            var firstSegment = firstBookingItem.Segments.First();
            Assert.IsNotNull(firstSegment.Flight.Carrier);
            Assert.IsFalse(string.IsNullOrEmpty(firstSegment.Flight.Carrier.Name));
            Assert.AreEqual(bookingResponse.Itinerary.OutboundLeg.Origin.Code, firstSegment.Origin.Code);
        }        
    }
}