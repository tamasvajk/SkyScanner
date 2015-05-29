// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Settings;

namespace SkyScanner.Test
{
    [TestClass]
    public class FlightTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Flights_Can_Be_Queried()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString("LOND-sky"),
                    Location.FromString("NYCA-sky"),
                    departureDate, departureDate.PlusDays(5)),
                new FlightResponseSettings()));

            Assert.AreNotEqual(0, itineraries.Count);
        }

        [TestMethod]
        public async Task Flights_One_Way()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);
            var outboundDepartureStartTime = new LocalTime(8, 0, 0);
            var outboundDepartureEndTime = new LocalTime(12, 0, 0);
            const int maxStops = 2;
            const int maxDuration = 14 * 60;

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString("GVA-sky"),
                    Location.FromString("JFK-sky"),
                    departureDate),
                new FlightResponseSettings(
                    sortOrder: SortOrder.Descending,
                    sortType: SortType.Price,
                    maxStops: maxStops,
                    maxDuration: maxDuration,
                    outboundDepartureStartTime: outboundDepartureStartTime,
                    outboundDepartureEndTime: outboundDepartureEndTime)));

            Assert.IsTrue(itineraries.Count > 0);

            foreach (var itinerary in itineraries)
            {
                Assert.IsNull(itinerary.InboundLeg);
            }
        }

        [TestMethod]
        public async Task Flights_Not_Morning()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);
            const int maxStops = 2;
            const int maxDuration = 18 * 60;

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString("GVA-sky"),
                    Location.FromString("LAX-sky"),
                    departureDate),
                new FlightResponseSettings(
                    sortOrder: SortOrder.Descending,
                    sortType: SortType.Price,
                    maxStops: maxStops,
                    maxDuration: maxDuration,
                    outboundDepartureTime: DayTimePeriod.Afternoon | DayTimePeriod.Evening)));

            Assert.IsTrue(itineraries.Count > 0);

            foreach (var itinerary in itineraries)
            {
                Assert.IsNull(itinerary.InboundLeg);
                Assert.IsTrue(itinerary.OutboundLeg.DepartureTime.TimeOfDay >= new LocalTime(12, 0));
            }
        }

        [TestMethod]
        public async Task Flights_Location_Schema()
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);
            var outboundDepartureStartTime = new LocalTime(8, 0, 0);
            var outboundDepartureEndTime = new LocalTime(12, 0, 0);
            const int maxStops = 2;
            const int maxDuration = 14 * 60;

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString("GVA-sky"),
                    Location.FromString("212.58.244.18"),
                    departureDate, locationSchema: LocationSchema.Ip),
                new FlightResponseSettings(
                    sortOrder: SortOrder.Descending,
                    sortType: SortType.Price,
                    maxStops: maxStops,
                    maxDuration: maxDuration,
                    outboundDepartureStartTime: outboundDepartureStartTime,
                    outboundDepartureEndTime: outboundDepartureEndTime)));

            foreach (var itinerary in itineraries)
            {
                Assert.IsNull(itinerary.InboundLeg);
            }
        }

        [TestMethod]
        public async Task Flights_Correspond_To_Query()
        {
            await CheckFlightsBetweenAirports("GIG-sky", "LHR-sky");
        }

        public async Task CheckFlightsBetweenAirports(string startCode, string endCode)
        {
            var departureDate = Instant.FromDateTimeUtc(DateTime.UtcNow).InUtc().Date.PlusMonths(1);
            var outboundDepartureStartTime = new LocalTime(8, 0, 0);
            var outboundDepartureEndTime = new LocalTime(12, 0, 0);
            var inboundDepartureStartTime = new LocalTime(8, 0, 0);
            var inboundDepartureEndTime = new LocalTime(18, 30, 0);
            const int maxStops = 2;
            const int maxDuration = 22 * 60;

            var itineraries = await Scanner.QueryFlight(new FlightQuerySettings(
                new FlightRequestSettings(
                    Location.FromString(startCode),
                    Location.FromString(endCode),
                    departureDate, departureDate.PlusDays(5)
                    ),
                new FlightResponseSettings(
                    sortOrder: SortOrder.Descending,
                    sortType: SortType.Price,
                    maxStops: maxStops,
                    maxDuration: maxDuration,
                    outboundDepartureStartTime: outboundDepartureStartTime,
                    outboundDepartureEndTime: outboundDepartureEndTime,
                    inboundDepartureStartTime: inboundDepartureStartTime,
                    inboundDepartureEndTime: inboundDepartureEndTime)));

            Assert.IsTrue(itineraries.Count > 0);

            foreach (var itinerary in itineraries)
            {
                Func<string, string> cleanCode = s => s.Replace("-sky", "");

                Assert.IsNotNull(itinerary.OutboundLeg.Origin.ParentPlace);
                Assert.IsNotNull(itinerary.OutboundLeg.Destination.ParentPlace);
                Assert.IsNotNull(itinerary.InboundLeg.Origin.ParentPlace);
                Assert.IsNotNull(itinerary.InboundLeg.Destination.ParentPlace);

                Assert.AreEqual(PlaceType.Airport, itinerary.OutboundLeg.Origin.Type);
                Assert.AreEqual(PlaceType.Airport, itinerary.OutboundLeg.Destination.Type);

                Assert.AreEqual(itinerary.InboundLeg.Destination.Code, itinerary.OutboundLeg.Origin.Code);
                Assert.AreEqual(itinerary.InboundLeg.Origin.Code, itinerary.OutboundLeg.Destination.Code);

                Assert.AreEqual(cleanCode(startCode), itinerary.OutboundLeg.Origin.Code);
                Assert.AreEqual(cleanCode(endCode), itinerary.OutboundLeg.Destination.Code);
                Assert.AreEqual(cleanCode(startCode), itinerary.InboundLeg.Destination.Code);
                Assert.AreEqual(cleanCode(endCode), itinerary.InboundLeg.Origin.Code);

                Assert.AreEqual(departureDate, itinerary.OutboundLeg.DepartureTime.Date);
                Assert.AreEqual(departureDate.PlusDays(5), itinerary.InboundLeg.DepartureTime.Date);
                
                Assert.IsTrue(itinerary.OutboundLeg.DepartureTime.TimeOfDay >= outboundDepartureStartTime);
                Assert.IsTrue(itinerary.OutboundLeg.DepartureTime.TimeOfDay <= outboundDepartureEndTime);

                Assert.IsTrue(itinerary.InboundLeg.DepartureTime.TimeOfDay >= inboundDepartureStartTime);
                Assert.IsTrue(itinerary.InboundLeg.DepartureTime.TimeOfDay <= inboundDepartureEndTime);
                
                Assert.AreEqual(Directionality.Outbound, itinerary.OutboundLeg.Directionality);
                Assert.AreEqual(Directionality.Inbound, itinerary.InboundLeg.Directionality);

                CheckLeg(itinerary.OutboundLeg, maxDuration, maxStops);
                CheckLeg(itinerary.InboundLeg, maxDuration, maxStops);

                foreach (var pricingOption in itinerary.PricingOptions)
                {
                    Assert.IsTrue(pricingOption.Agents.Any());
                }
            }

            CheckDescendingPriceOrdering(itineraries);
        }

        private static void CheckDescendingPriceOrdering(IEnumerable<Itinerary> itineraries)
        {
            decimal? previousPrice = null;
            foreach (var itinerary in itineraries)
            {
                var price = itinerary.PricingOptions.Select(option => option.Price).Min();

                if (previousPrice.HasValue)
                {
                    Assert.IsTrue(Math.Floor(price) - Math.Floor(previousPrice.Value) <= 1);
                }
                previousPrice = Math.Floor(price);
            }
        }

        private static void CheckLeg(Leg leg, int maxDuration, int maxStops)
        {
            Assert.IsTrue(leg.Duration <= maxDuration);
            Assert.IsTrue(leg.Stops.Count <= maxStops);

            Assert.AreEqual(JourneyMode.Flight, leg.JourneyMode);

            Assert.AreEqual(leg.Stops.Count + 1, leg.Segments.Count);
            Assert.AreEqual(leg.Segments.Count, leg.FlightInfos.Count);

            Assert.IsTrue(leg.Carriers.Any());
            Assert.IsTrue(leg.OperatingCarriers.Any());

            var segments = leg.Segments.ToList();
            var stops = leg.Stops.ToList();
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = leg.Segments.ToList()[i];
                if (i == 0)
                {
                    Assert.AreEqual(leg.Origin.Code, segment.Origin.Code);
                }
                else
                {
                    Assert.AreEqual(stops[i - 1].Code, segment.Origin.Code);
                }

                if (i == segments.Count - 1)
                {
                    Assert.AreEqual(leg.Destination.Code, segment.Destination.Code);
                }
                else
                {
                    Assert.AreEqual(stops[i].Code, segment.Destination.Code);
                    Assert.AreEqual(segment.Destination.Code, segments[i + 1].Origin.Code);
                }

                foreach (var flightInfo in leg.FlightInfos)
                {
                    Assert.IsNotNull(flightInfo.Carrier);
                }

                Assert.IsNotNull(segment.Flight.Carrier);
            }
        }
    }
}
