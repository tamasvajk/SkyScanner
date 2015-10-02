using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyScanner.Booking;
using SkyScanner.Data;
using SkyScanner.Data.Interim;
using SkyScanner.Settings;
using System.Threading;

namespace SkyScanner.Services.Interfaces
{
    public interface IScanner
    {
        Task<List<Itinerary>> QueryFlight(FlightQuerySettings flightQuerySettings,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Itinerary>> QueryFlight(FlightQuerySettings flightQuerySettings,
            Action<InterimChangeSet<Itinerary>> interimResultCallback,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<BookingResponse> QueryBooking(Itinerary itinerary,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Data.Currency>> QueryCurrency(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Data.Market>> QueryMarket(Data.Locale locale,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Location>> QueryLocation(LocationAutosuggestSettings settings,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Location>> QueryLocation(string query,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Data.Locale>> QueryLocale(CancellationToken cancellationToken = default(CancellationToken));
    }
}