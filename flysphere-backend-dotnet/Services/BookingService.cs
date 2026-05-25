using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IFlightService _flightService;
        private readonly FlysphereBackendDotnet.Data.ApplicationDbContext _context;

        public BookingService(
            IBookingRepository bookingRepository,
            IPassengerRepository passengerRepository,
            IFlightService flightService,
            FlysphereBackendDotnet.Data.ApplicationDbContext context)
        {
            _bookingRepository = bookingRepository;
            _passengerRepository = passengerRepository;
            _flightService = flightService;
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(
            long userId,
            long outboundFlightId,
            long? returnFlightId,
            List<Passenger> passengers,
            double totalAmount,
            string outboundCabinClass,
            string returnCabinClass,
            string contactEmail,
            string contactPhone,
            FlysphereBackendDotnet.Controllers.BookingController.SegmentAddOns outboundAddOns,
            FlysphereBackendDotnet.Controllers.BookingController.SegmentAddOns returnAddOns)
        {
            string bookingCode = "FS" + Guid.NewGuid().ToString("N").Substring(0, 6);

            var booking = new Booking
            {
                BookingId = bookingCode,
                UserId = userId,
                FlightId = outboundFlightId,
                TotalAmount = totalAmount,
                // ✅ Prevent NULL constraint failure
                ContactEmail = string.IsNullOrWhiteSpace(contactEmail) ? "unknown@flysphere.com" : contactEmail,
                ContactPhone = string.IsNullOrWhiteSpace(contactPhone) ? "0000000000" : contactPhone,
                Status = "CONFIRMED",
                CreatedAt = DateTime.UtcNow
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            booking = await _bookingRepository.AddAsync(booking);

            booking.Segments = new List<BookingSegment>();

            var outboundFlight = await _flightService.GetFlightByIdAsync(outboundFlightId);

            var outboundSegment = new BookingSegment
            {
                BookingId = booking.Id,
                SegmentNo = 1,
                FlightId = outboundFlight.Id,
                CabinType = outboundCabinClass,
                SeatPreference = outboundAddOns?.SeatPreference,
                MealPreference = outboundAddOns?.MealPreference,
                ExtraBaggage = outboundAddOns?.ExtraBaggage,
                TravelProtection = outboundAddOns?.TravelProtection
            };

            booking.Segments.Add(outboundSegment);

            if (returnFlightId.HasValue)
            {
                var returnFlight = await _flightService.GetFlightByIdAsync(returnFlightId.Value);

                string returnClassToUse = !string.IsNullOrEmpty(returnCabinClass)
                    ? returnCabinClass
                    : outboundCabinClass;

                var returnSegment = new BookingSegment
                {
                    BookingId = booking.Id,
                    SegmentNo = 2,
                    FlightId = returnFlight.Id,
                    CabinType = returnClassToUse,
                    SeatPreference = returnAddOns?.SeatPreference,
                    MealPreference = returnAddOns?.MealPreference,
                    ExtraBaggage = returnAddOns?.ExtraBaggage,
                    TravelProtection = returnAddOns?.TravelProtection
                };

                booking.Segments.Add(returnSegment);
            }

            await _bookingRepository.UpdateAsync(booking);

            int seatsToBook = passengers.Count;

            await _flightService.DecrementSeatsAsync(outboundFlightId, outboundCabinClass, seatsToBook);

            if (returnFlightId.HasValue)
            {
                string returnClassToUse = !string.IsNullOrEmpty(returnCabinClass)
                    ? returnCabinClass
                    : outboundCabinClass;

                await _flightService.DecrementSeatsAsync(returnFlightId.Value, returnClassToUse, seatsToBook);
            }

            foreach (var passenger in passengers)
            {
                passenger.BookingId = booking.Id;
                await _passengerRepository.AddAsync(passenger);
            }

            await transaction.CommitAsync();
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(long userId)
        {
            return await _bookingRepository.GetByUserIdAsync(userId);
        }

        public async Task CancelBookingAsync(string bookingCode)
        {
            var booking = await _bookingRepository.GetByBookingIdAsync(bookingCode);

            if (booking == null)
                throw new Exception("Booking not found");

            booking.Status = "CANCELLED";
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<Booking> GetBookingByCodeAsync(string bookingId)
        {
            var booking = await _bookingRepository.GetByBookingIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            return booking;
        }
    }
}
