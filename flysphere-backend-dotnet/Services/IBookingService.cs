using FlysphereBackendDotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(
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
            FlysphereBackendDotnet.Controllers.BookingController.SegmentAddOns returnAddOns);

        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(long userId);
        Task CancelBookingAsync(string bookingCode);
        Task<Booking> GetBookingByCodeAsync(string bookingId);
    }
}
