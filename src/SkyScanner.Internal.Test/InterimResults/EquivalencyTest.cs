using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyScanner.Data;
using SkyScanner.Data.Comparison;

namespace SkyScanner.Test.InterimResults
{
    [TestClass]
    public class EquivalencyTest
    {
        [TestMethod]
        public void Itinerary_ShouldBeEqual_WithOtherItineraryWithEqualLegs()
        {
            var option = new PricingOption { Price = 123M, QuoteAge = 1, AgentIds = { 12 } };

            var first = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option } };
            var second = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option } };

            var comparer = new ShallowEqualityComparer<Itinerary>();
            Assert.IsTrue(comparer.Equals(first, second));
        }

        [TestMethod]
        public void Itinerary_ShouldNotBeEqual_WithOtherItineraryWithDifferentLegs()
        {
            var option = new PricingOption { Price = 123M, QuoteAge = 1, AgentIds = { 12 } };

            var first = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option } };
            var second = new Itinerary { OutboundLegId = "BBB", InboundLegId = "CCC", PricingOptions = { option } };

            var comparer = new ShallowEqualityComparer<Itinerary>();
            Assert.IsFalse(comparer.Equals(first, second));
        }

        [TestMethod]
        public void Itinerary_ShouldBeEqual_IfPricingOptionWasAddedAndComparingShallow()
        {
            var option1 = new PricingOption { Price = 123M, QuoteAge = 1, AgentIds = { 12 } };
            var option2 = new PricingOption { Price = 456M, QuoteAge = 1, AgentIds = { 13 } };

            var first = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option1 } };
            var second = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option1, option2 } };

            var comparer = new ShallowEqualityComparer<Itinerary>();
            Assert.IsTrue(comparer.Equals(first, second));
        }

        [TestMethod]
        public void Itinerary_ShouldNotBeEqual_IfPricingOptionWasAddedAndComparingDeep()
        {
            var option1 = new PricingOption { Price = 123M, QuoteAge = 1, AgentIds = { 12 } };
            var option2 = new PricingOption { Price = 456M, QuoteAge = 1, AgentIds = { 13 } };

            var first = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option1 } };
            var second = new Itinerary { OutboundLegId = "AAA", InboundLegId = "BBB", PricingOptions = { option1, option2 } };

            Assert.IsFalse(((IInterimEquatable<Itinerary>)first).DeepEquals(second));
        }
    }
}