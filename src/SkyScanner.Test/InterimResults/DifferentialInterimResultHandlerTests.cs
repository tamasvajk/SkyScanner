namespace SkyScanner.Test.InterimResults
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SkyScanner.Services.Helpers;

    [DeploymentItem("data")]
    [TestClass]
    public class DifferentialInterimResultHandlerTests : HandlerTestBase
    {
        [TestMethod]
        public void DifferentialInterimResultHandler_ShouldReturnResponse_OnFirstCall()
        {
            var handler = new DifferentialInterimFlightResultHandler();

            var response = DeserializeResponse("response_1.json");

            var result = handler.Handle(response);

            Assert.AreEqual(result.All.Count(), 6);
            Assert.AreEqual(result.Additions.Count(), 0);
            Assert.AreEqual(result.Updates.Count(), 0);

        }

        [TestMethod]
        public void DifferentialInterimResultHandler_ShouldReturnDiff_OnSubsequentCall()
        {
            var handler = new DifferentialInterimFlightResultHandler();

            var response1 = DeserializeResponse("response_1.json");
            var response2 = DeserializeResponse("response_2.json");

            var ignore = handler.Handle(response1);
            var result = handler.Handle(response2);

            Assert.AreEqual(result.All.Count(), 8);
            Assert.AreEqual(result.Additions.Count(), 2);
            Assert.AreEqual(result.Updates.Count(), 0);
        }

        [TestMethod]
        public void DifferentialInterimResultHandler_ShouldReturnDiff_OnSubsequentCall2()
        {
            var handler = new DifferentialInterimFlightResultHandler();

            var response1 = DeserializeResponse("response_1.json");
            var response2 = DeserializeResponse("response_2.json");
            var response3 = DeserializeResponse("response_3.json");
            var response4 = DeserializeResponse("response_4.json");
            var response5 = DeserializeResponse("response_5.json");
            var response6 = DeserializeResponse("response_5.json");

            var ignore = handler.Handle(response1);
            ignore = handler.Handle(response2);
            ignore = handler.Handle(response3);
            ignore = handler.Handle(response4);
            ignore = handler.Handle(response5);
            ignore = handler.Handle(response6);
            // Check test output
        }
    }
}