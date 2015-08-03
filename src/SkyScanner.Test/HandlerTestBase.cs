namespace SkyScanner.Test
{
    using System.IO;

    using Newtonsoft.Json;

    using SkyScanner.Flight;
    using SkyScanner.Services;

    public class HandlerTestBase
    {
        internal static FlightResponse DeserializeResponse(string filename)
        {
            var result = JsonConvert.DeserializeObject<FlightResponse>(
                File.ReadAllText(filename),
                Scanner.JsonSerializerSettings);

            result.Itineraries.ForEach(itinerary => { itinerary.FlightResponse = result; });
            result.Legs.ForEach(leg => { leg.FlightResponse = result; });
            result.Segments.ForEach(segment => { segment.ContainerResponse = result; });
            result.Places.ForEach(place => { place.ContainerResponse = result; });

            return result;
        }
    }
}