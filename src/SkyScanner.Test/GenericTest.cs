// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Services;

namespace SkyScanner.Test
{
    [TestClass]
    public class GenericTest
    {
        [TestMethod]
        public async Task Scanner_Wrong_Key()
        {
            var scanner = new Scanner("SomeWrongKey");
            try
            {
                var currencies = await scanner.QueryCurrency();
                Assert.Fail("Did not throw on wrong key");
            }
            catch (Exception)
            {
            }
        }
    }
}