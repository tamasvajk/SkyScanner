// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Data;
using SkyScanner.Settings;

namespace SkyScanner.Test
{
    [TestClass]
    public class LocationTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Locations_Can_Be_Queried()
        {
            var locations = await Scanner.QueryLocation(new LocationAutosuggestSettings("London"));
            Assert.IsNotNull(locations);
            Assert.AreNotEqual(0, locations.Count);
        }

        [TestMethod]
        public async Task Locations_Can_Be_Queried_With_Different_Settings()
        {
            var locales = await Scanner.QueryLocale();
            var locationsEng = await Scanner.QueryLocation(new LocationAutosuggestSettings("London", 
                LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, locales.First(locale => locale.Code == "en-GB")));

            var locationsFr = await Scanner.QueryLocation(new LocationAutosuggestSettings("London", 
                LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, locales.First(locale => locale.Code == "fr-FR")));

            Assert.AreEqual(locationsEng.Count, locationsFr.Count);

            var locationsHeathrow = await Scanner.QueryLocation(new LocationAutosuggestSettings("LHR-sky", 
                LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, locales.First(locale => locale.Code == "fr-FR")));

            Assert.AreEqual(1, locationsHeathrow.Count);

            var ipBased = await Scanner.QueryLocation(new LocationAutosuggestSettings("212.58.244.18-IP", 
                LocationAutosuggestQueryType.Query, Market.Default, Currency.Default, locales.First(locale => locale.Code == "fr-FR")));

            Assert.AreEqual(1, ipBased.Count);
            Assert.AreEqual("LOND-sky", ipBased.First().PlaceId);

            var info = await Scanner.QueryLocation(new LocationAutosuggestSettings("LOND-sky", 
                LocationAutosuggestQueryType.Id, Market.Default, Currency.Default, locales.First(locale => locale.Code == "fr-FR")));

            Assert.AreEqual(1, info.Count);
        }
    }
}