using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public interface IBookingSegmentRepository
    {
        Task<BookingSegment> GetByIdAsync(long id);
        Task<IEnumerable<BookingSegment>> GetByBookingIdAsync(long bookingId);
        Task<bool> ExistsByFlightIdAsync(long flightId);
        Task<BookingSegment> AddAsync(BookingSegment segment);
        Task DeleteAsync(long id);
    }
}
