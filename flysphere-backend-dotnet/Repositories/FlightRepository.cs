using FlysphereBackendDotnet.Data;
using FlysphereBackendDotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetByIdAsync(long id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<IEnumerable<Flight>> SearchAsync(string departureAirport, string arrivalAirport, System.DateTime departureDate)
        {
            return await _context.Flights
                .Where(f =>
                    f.DepartureAirport == departureAirport &&
                    f.ArrivalAirport == arrivalAirport &&
                    f.DepartureDate.Value.Date == departureDate.Date)
                .ToListAsync();
        }

        public async Task<Flight> AddAsync(Flight flight)
        {
            // ✅ Ensure aircraft_type is never null (DB has NOT NULL constraint)
            if (string.IsNullOrWhiteSpace(flight.AircraftType))
            {
                flight.AircraftType = "N/A";
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task UpdateAsync(Flight flight)
        {
            // ✅ Ensure aircraft_type is never null during update
            if (string.IsNullOrWhiteSpace(flight.AircraftType))
            {
                flight.AircraftType = "N/A";
            }

            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }
    }
}
