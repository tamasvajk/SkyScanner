using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyScanner.Booking;
using SkyScanner.Data;
using SkyScanner.Data.Interim;
using SkyScanner.Settings;

namespace SkyScanner.Services.Interfaces
{
    public interface IScanner
    {
        Task<List<Itinerary>> QueryFlight(FlightQuerySettings flightQuerySettings);
        Task<List<Itinerary>> QueryFlight(FlightQuerySettings flightQuerySettings, Action<InterimChangeSet<Itinerary>> interimResultCallback);
        Task<BookingResponse> QueryBooking(Itinerary itinerary);
        Task<List<Data.Currency>> QueryCurrency();
        Task<List<Data.Market>> QueryMarket(Data.Locale locale);
        Task<List<Location>> QueryLocation(LocationAutosuggestSettings settings);
        Task<List<Location>> QueryLocation(string query);
        Task<List<Data.Locale>> QueryLocale();
    }
}