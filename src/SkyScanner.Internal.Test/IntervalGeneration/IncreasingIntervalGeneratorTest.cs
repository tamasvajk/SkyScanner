using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Services.Helpers;

namespace SkyScanner.Test.IntervalGeneration
{
    [TestClass]
    public class IncreasingIntervalGeneratorTest
    {
        [TestMethod]
        public void IncreasingIntervalGenerator_ShouldReturnIncreasingTaskDelays()
        {
            var intervalGenerator = new IncreasingIntervalGenerator();
            Assert.AreEqual(intervalGenerator.NextInterval, 0);
            Assert.AreEqual(intervalGenerator.NextInterval, 1000);
            Assert.AreEqual(intervalGenerator.NextInterval, 2000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
            Assert.AreEqual(intervalGenerator.NextInterval, 3000);
        }
    }
}