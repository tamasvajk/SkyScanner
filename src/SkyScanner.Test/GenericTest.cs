// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Services;

namespace SkyScanner.Test
{
    [TestClass]
    public class GenericTest
    {
        private Scanner _scanner;

        [TestInitialize]
        public void Setup()
        {
            _scanner = new Scanner("SomeWrongKey");
        }

        [TestMethod]
        public async Task Scanner_Wrong_Key()
        {
            try
            {
                await _scanner.QueryCurrency();
                Assert.Fail("Did not throw on wrong key");
            }
            catch (Exception)
            {
                Debug.WriteLine("Throwed exception on wrong key");
            }
        }
    }
}