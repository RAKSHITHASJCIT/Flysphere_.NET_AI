using FlysphereBackendDotnet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public interface IFlightService
    {
        Task<Flight> CreateFlightAsync(Flight flight);
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<Flight> GetFlightByIdAsync(long id);
        Task<Flight> UpdateFlightAsync(long id, Flight updatedFlight);
        Task<Flight> CancelFlightAsync(long id);
        Task DeleteFlightAsync(long id);
        Task<IEnumerable<Flight>> SearchFlightsAsync(string from, string to, DateTime date);
        Task DecrementSeatsAsync(long flightId, string cabinClass, int seatsToBook);
    }
}
