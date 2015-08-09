// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Data;

namespace SkyScanner.Test
{
    [TestClass]
    public class MarketTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Markets_Can_Be_Queried()
        {
            var markets = await Scanner.QueryMarket(Locale.Default);
            Assert.IsNotNull(markets);
            Assert.AreNotEqual(0, markets.Count);
        }

        [TestMethod]
        public async Task Default_Market_Exists()
        {
            var markets = await Scanner.QueryMarket(Locale.Default);
            Assert.IsNotNull(markets.SingleOrDefault(market => market.Code == Market.Default.Code));
        }
    }
}