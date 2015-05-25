// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SkyScanner.Test
{
    [TestClass]
    public class LocaleTest : TestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public async Task Locales_Can_Be_Queried()
        {
            var locales = await Scanner.QueryLocale();
            Assert.IsNotNull(locales);
            Assert.AreNotEqual(0, locales.Count);
        }

        [TestMethod]
        public async Task Default_Locale_Exists()
        {
            var locales = await Scanner.QueryLocale();
            Assert.IsNotNull(locales.SingleOrDefault(locale => locale.Code == Data.Locale.Default.Code));
        }
    }
}