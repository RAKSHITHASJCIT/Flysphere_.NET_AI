using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public interface IFlightRepository
    {
        Task<Flight> GetByIdAsync(long id);
        Task<IEnumerable<Flight>> GetAllAsync();
        Task<IEnumerable<Flight>> SearchAsync(string departureAirport, string arrivalAirport, System.DateTime departureDate);
        Task<Flight> AddAsync(Flight flight);
        Task UpdateAsync(Flight flight);
        Task DeleteAsync(long id);
    }
}
