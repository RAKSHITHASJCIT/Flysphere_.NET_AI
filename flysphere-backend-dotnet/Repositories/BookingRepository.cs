using FlysphereBackendDotnet.Data;
using FlysphereBackendDotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> GetByIdAsync(long id)
        {
            return await _context.Bookings
                .Include(b => b.Segments)
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> GetByBookingIdAsync(string bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Segments)
                    .ThenInclude(s => s.Flight) // ✅ Load Flight details for confirmation page
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Segments)
                    .ThenInclude(s => s.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(long userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Segments)
                    .ThenInclude(s => s.Flight)
                .Include(b => b.Passengers)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}
