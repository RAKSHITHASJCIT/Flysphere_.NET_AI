using FlysphereBackendDotnet.Data;
using FlysphereBackendDotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public class BookingSegmentRepository : IBookingSegmentRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingSegmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingSegment> GetByIdAsync(long id)
        {
            return await _context.BookingSegments
                .Include(s => s.Flight)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<BookingSegment>> GetByBookingIdAsync(long bookingId)
        {
            return await _context.BookingSegments
                .Where(s => s.BookingId == bookingId)
                .Include(s => s.Flight)
                .ToListAsync();
        }

        public async Task<bool> ExistsByFlightIdAsync(long flightId)
        {
            return await _context.BookingSegments
                .AnyAsync(s => s.FlightId == flightId);
        }

        public async Task<BookingSegment> AddAsync(BookingSegment segment)
        {
            _context.BookingSegments.Add(segment);
            await _context.SaveChangesAsync();
            return segment;
        }

        public async Task DeleteAsync(long id)
        {
            var segment = await _context.BookingSegments.FindAsync(id);
            if (segment != null)
            {
                _context.BookingSegments.Remove(segment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
