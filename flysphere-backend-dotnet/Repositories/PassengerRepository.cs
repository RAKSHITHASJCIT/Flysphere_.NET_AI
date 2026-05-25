using FlysphereBackendDotnet.Data;
using FlysphereBackendDotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly ApplicationDbContext _context;

        public PassengerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Passenger> GetByIdAsync(long id)
        {
            return await _context.Passengers.FindAsync(id);
        }

        public async Task<IEnumerable<Passenger>> GetByBookingIdAsync(long bookingId)
        {
            return await _context.Passengers
                .Where(p => p.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<Passenger> AddAsync(Passenger passenger)
        {
            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();
            return passenger;
        }

        public async Task DeleteAsync(long id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
                await _context.SaveChangesAsync();
            }
        }
    }
}
