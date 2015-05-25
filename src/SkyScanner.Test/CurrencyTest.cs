// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkyScanner.Test
{
    [TestClass]
    public class CurrencyTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Currencies_Can_Be_Queried()
        {
            var currencies = await Scanner.QueryCurrency();
            Assert.IsNotNull(currencies);
            Assert.AreNotEqual(0, currencies.Count);
        }

        [TestMethod]
        public async Task Default_Currency_Exists()
        {
            var currencies = await Scanner.QueryCurrency();
            Assert.IsNotNull(currencies.SingleOrDefault(currency => currency.Code == Data.Currency.Default.Code));
        }

        [TestMethod]
        public async Task Currency_Formatter()
        {
            Assert.AreEqual("£12.20", Data.Currency.Default.FormatValue(12.2m));

            var currencies = await Scanner.QueryCurrency();
            var chf = currencies.Single(currency => currency.Code == "CHF");
            Assert.AreEqual("Fr. 12.20", chf.FormatValue(12.2m));
        }
    }
}