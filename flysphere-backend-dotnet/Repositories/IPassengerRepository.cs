using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public interface IPassengerRepository
    {
        Task<Passenger> GetByIdAsync(long id);
        Task<IEnumerable<Passenger>> GetByBookingIdAsync(long bookingId);
        Task<Passenger> AddAsync(Passenger passenger);
        Task DeleteAsync(long id);
    }
}
