using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(long id);
        Task<Booking> GetByBookingIdAsync(string bookingId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByUserIdAsync(long userId);
        Task<Booking> AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(long id);
    }
}
