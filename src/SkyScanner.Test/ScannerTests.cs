namespace SkyScanner.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SkyScanner.Services;

    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScannerConstructor_ShouldThrowException_OnNullValueForApiKey()
        {
            var scanner = new Scanner(null);
        }         
    }
}