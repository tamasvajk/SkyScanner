// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using SkyScanner.Data;
using SkyScanner.Settings;
using SkyScanner.Booking;

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
        public async Task Bookings_Can_Be_Queried_With_Itinerary()
        {
            var itinerary = await GetItineraryForBookingQuery();
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

        [TestMethod]
        public async Task Bookings_Can_Be_Queried_With_Parameters()
        {
            var itinerary = await GetItineraryForBookingQuery();
            var bookingResponse = await Scanner.QueryBooking(
                itinerary.SessionKey, itinerary.OutboundLeg.Id, itinerary.InboundLeg.Id);

            Assert.IsNotNull(bookingResponse);
            Assert.AreNotEqual(0, bookingResponse.BookingOptions.Count);
            var firstBookingItem = bookingResponse.BookingOptions.First().BookingItems.First();

            //we don't have agent info if we went through the parametered booking query
            Assert.IsNull(firstBookingItem.Agent);
            Assert.AreNotEqual(0, firstBookingItem.Segments.Count);

            var firstSegment = firstBookingItem.Segments.First();
            Assert.IsNotNull(firstSegment.Flight.Carrier);
            Assert.IsFalse(string.IsNullOrEmpty(firstSegment.Flight.Carrier.Name));

            Assert.AreEqual(itinerary.OutboundLeg.Origin.Code, firstSegment.Origin.Code);
        }

        private async Task<Itinerary> GetItineraryForBookingQuery()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString("MAD-sky"),
                    Location.FromString("WASA-sky"),
                    departureDate, departureDate.PlusDays(5)),
                new FlightResponseSettings()));

            var itinerary = itineraries.First();
            return itinerary;
        }
    }
}