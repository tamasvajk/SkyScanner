using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Data;
using SkyScanner.Flight;
using SkyScanner.Services.Helpers;

namespace SkyScanner.Test.InterimResults
{
    [DeploymentItem("data")]
    [TestClass]
    public class InterimResultProviderTest : HandlerTestBase
    {
        [TestMethod]
        public void InterimResultProvider_ShouldReturnResponse_OnFirstCall()
        {
            var handler = new InterimResultProvider<FlightResponse, Itinerary>();

            var response = DeserializeResponse("response_1.json");

            var result = handler.Calculate(response);

            Assert.AreEqual(6, result.All.Count());
            Assert.AreEqual(0, result.Additions.Count());
            Assert.AreEqual(0, result.Updates.Count());

        }

        [TestMethod]
        public void InterimResultProvider_ShouldReturnDiff_OnSubsequentCall()
        {
            var handler = new InterimResultProvider<FlightResponse, Itinerary>();

            var response1 = DeserializeResponse("response_1.json");
            var response2 = DeserializeResponse("response_2.json");

            var result1 = handler.Calculate(response1);
            var result2 = handler.Calculate(response2);
            
            Assert.AreEqual(8, result2.All.Count());
            Assert.AreEqual(2, result2.Additions.Count());
            
            Assert.AreEqual(0, result2.Updates.Count());

            var refEqualCount = result2.All.Count(n => result1.All.Contains(n));
            Assert.AreEqual(6, refEqualCount);
        }

        [TestMethod]
        public void InterimResultProvider_ShouldReturnDiff_OnSubsequentCall_Many()
        {
            var handler = new InterimResultProvider<FlightResponse, Itinerary>();

            var response1 = DeserializeResponse("response_1.json");
            var response2 = DeserializeResponse("response_2.json");
            var response3 = DeserializeResponse("response_3.json");
            var response4 = DeserializeResponse("response_4.json");
            var response5 = DeserializeResponse("response_5.json");
            var response6 = DeserializeResponse("response_6.json");

            var ignore = handler.Calculate(response1);
            ignore = handler.Calculate(response2);
            ignore = handler.Calculate(response3);
            ignore = handler.Calculate(response4);
            ignore = handler.Calculate(response5);
            ignore = handler.Calculate(response6);
            // Check test output
        }
    }
}