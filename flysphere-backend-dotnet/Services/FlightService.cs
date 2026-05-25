using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingSegmentRepository _bookingSegmentRepository;

        public FlightService(IFlightRepository flightRepository,
                             IBookingSegmentRepository bookingSegmentRepository)
        {
            _flightRepository = flightRepository;
            _bookingSegmentRepository = bookingSegmentRepository;
        }

        public async Task<Flight> CreateFlightAsync(Flight flight)
        {
            // ✅ Check if flight already exists with same route + date + time
            var existingFlights = await _flightRepository.GetAllAsync();

            var duplicate = existingFlights.FirstOrDefault(f =>
                f.AirlineName == flight.AirlineName &&
                f.FlightType == flight.FlightType &&
                f.DepartureAirport == flight.DepartureAirport &&
                f.ArrivalAirport == flight.ArrivalAirport &&
                f.DepartureDate.HasValue && flight.DepartureDate.HasValue &&
                f.DepartureDate.Value.Date == flight.DepartureDate.Value.Date &&
                f.DepartureTime == flight.DepartureTime
            );

            if (duplicate != null)
            {
                throw new Exception("Flight already exists with same airline, flight type, route, date and time");
            }

            return await _flightRepository.AddAsync(flight);
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            var flights = (await _flightRepository.GetAllAsync()).ToList();
            var today = DateTime.Today;

            foreach (var flight in flights)
            {
                if (flight.DepartureDate.HasValue &&
                    flight.DepartureDate.Value.Date < today &&
                    !string.Equals(flight.FlightStatus, "Cancelled", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(flight.FlightStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    flight.FlightStatus = "Completed";
                    await _flightRepository.UpdateAsync(flight);
                }
            }

            return flights;
        }

        public async Task<Flight> GetFlightByIdAsync(long id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null)
                throw new Exception("Flight not found");

            return flight;
        }

        public async Task<Flight> UpdateFlightAsync(long id, Flight updatedFlight)
        {
            var flight = await GetFlightByIdAsync(id);

            if (updatedFlight.AirlineName != null)
                flight.AirlineName = updatedFlight.AirlineName;

            if (updatedFlight.FlightType != null)
                flight.FlightType = updatedFlight.FlightType;

            if (updatedFlight.DepartureAirport != null)
                flight.DepartureAirport = updatedFlight.DepartureAirport;

            if (updatedFlight.ArrivalAirport != null)
                flight.ArrivalAirport = updatedFlight.ArrivalAirport;

            if (updatedFlight.DepartureDate != null)
                flight.DepartureDate = updatedFlight.DepartureDate;

            if (updatedFlight.ArrivalDate != null)
                flight.ArrivalDate = updatedFlight.ArrivalDate;

            if (updatedFlight.DepartureTime != null)
                flight.DepartureTime = updatedFlight.DepartureTime;

            if (updatedFlight.ArrivalTime != null)
                flight.ArrivalTime = updatedFlight.ArrivalTime;

            if (updatedFlight.FlightNo != null)
                flight.FlightNo = updatedFlight.FlightNo;

            if (updatedFlight.TotalEconomySeats != null)
                flight.TotalEconomySeats = updatedFlight.TotalEconomySeats;

            if (updatedFlight.TotalBusinessSeats != null)
                flight.TotalBusinessSeats = updatedFlight.TotalBusinessSeats;

            if (updatedFlight.TotalFirstClassSeats != null)
                flight.TotalFirstClassSeats = updatedFlight.TotalFirstClassSeats;

            if (updatedFlight.EconomyAdultFare != null)
                flight.EconomyAdultFare = updatedFlight.EconomyAdultFare;

            if (updatedFlight.EconomyChildFare != null)
                flight.EconomyChildFare = updatedFlight.EconomyChildFare;

            if (updatedFlight.BusinessAdultFare != null)
                flight.BusinessAdultFare = updatedFlight.BusinessAdultFare;

            if (updatedFlight.BusinessChildFare != null)
                flight.BusinessChildFare = updatedFlight.BusinessChildFare;

            if (updatedFlight.FirstAdultFare != null)
                flight.FirstAdultFare = updatedFlight.FirstAdultFare;

            if (updatedFlight.FirstChildFare != null)
                flight.FirstChildFare = updatedFlight.FirstChildFare;

            if (updatedFlight.AircraftType != null)
                flight.AircraftType = updatedFlight.AircraftType;

            if (updatedFlight.FlightStatus != null)
                flight.FlightStatus = updatedFlight.FlightStatus;

            await _flightRepository.UpdateAsync(flight);
            return flight;
        }

        public async Task<Flight> CancelFlightAsync(long id)
        {
            var flight = await GetFlightByIdAsync(id);

            if (string.Equals(flight.FlightStatus, "Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Flight is already cancelled");

            flight.FlightStatus = "Cancelled";
            await _flightRepository.UpdateAsync(flight);

            return flight;
        }

        public async Task DeleteFlightAsync(long id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null)
                throw new Exception("Flight not found");

            var exists = await _bookingSegmentRepository.ExistsByFlightIdAsync(id);
            if (exists)
                throw new Exception("Cannot delete flight with existing bookings");

            await _flightRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Flight>> SearchFlightsAsync(string from, string to, DateTime date)
        {
            var flights = (await _flightRepository.SearchAsync(from.Trim(), to.Trim(), date)).ToList();
            var today = DateTime.Today;

            foreach (var flight in flights)
            {
                if (flight.DepartureDate.HasValue &&
                    flight.DepartureDate.Value.Date < today &&
                    !string.Equals(flight.FlightStatus, "Cancelled", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(flight.FlightStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    flight.FlightStatus = "Completed";
                    await _flightRepository.UpdateAsync(flight);
                }
            }

            return flights;
        }

        public async Task DecrementSeatsAsync(long flightId, string cabinClass, int seatsToBook)
        {
            var flight = await GetFlightByIdAsync(flightId);

            switch (cabinClass.ToLower())
            {
                case "economy":
                    if (flight.TotalEconomySeats < seatsToBook)
                        throw new Exception("Not enough Economy seats");
                    flight.TotalEconomySeats -= seatsToBook;
                    break;

                case "business":
                    if (flight.TotalBusinessSeats < seatsToBook)
                        throw new Exception("Not enough Business seats");
                    flight.TotalBusinessSeats -= seatsToBook;
                    break;

                case "first":
                case "first class":
                    if (flight.TotalFirstClassSeats < seatsToBook)
                        throw new Exception("Not enough First Class seats");
                    flight.TotalFirstClassSeats -= seatsToBook;
                    break;

                default:
                    throw new Exception("Invalid cabin class");
            }

            await _flightRepository.UpdateAsync(flight);
        }
    }
}
