// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Data;
using System.Threading;
using System;

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
        public async Task Currency_Query_Can_Be_Cancelled()
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                var token = tokenSource.Token;

                try
                {
                    tokenSource.Cancel();
                    await Scanner.QueryCurrency(token);
                    Assert.Fail();
                }
                catch (OperationCanceledException exc)
                {
                    Assert.AreEqual(token, exc.CancellationToken);
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public async Task Default_Currency_Exists()
        {
            var currencies = await Scanner.QueryCurrency();
            Assert.IsNotNull(currencies.SingleOrDefault(currency => currency.Code == Currency.Default.Code));
        }

        [TestMethod]
        public async Task Currency_Formatter()
        {
            Assert.AreEqual("£12.20", Currency.Default.FormatValue(12.2M));

            var currencies = await Scanner.QueryCurrency();
            var chf = currencies.Single(currency => currency.Code == "CHF");
            Assert.AreEqual("CHF 12.20", chf.FormatValue(12.2M));

            var gbp = currencies.Single(currency => currency.Code == "GBP");
            Assert.AreEqual("£12.20", gbp.FormatValue(12.2M));
        }
    }
}