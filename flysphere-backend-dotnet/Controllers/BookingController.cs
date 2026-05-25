using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingRequest request)
        {
            var booking = await _bookingService.CreateBookingAsync(
                request.UserId,
                request.OutboundFlightId,
                request.ReturnFlightId,
                request.Passengers,
                request.TotalAmount,
                request.OutboundCabinClass,
                request.ReturnCabinClass,
                request.ContactEmail,
                request.ContactPhone,
                request.OutboundAddOns,
                request.ReturnAddOns
            );

            return Ok(booking);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByUser(long userId)
        {
            var bookings = await _bookingService.GetBookingsByUserAsync(userId);
            return Ok(bookings);
        }

        [HttpPut("{bookingCode}/cancel")]
        public async Task<ActionResult> CancelBooking(string bookingCode)
        {
            await _bookingService.CancelBookingAsync(bookingCode);
            return Ok("Booking cancelled successfully");
        }

        [HttpGet("{bookingCode}")]
        public async Task<ActionResult<Booking>> GetBooking(string bookingCode)
        {
            var booking = await _bookingService.GetBookingByCodeAsync(bookingCode);
            return Ok(booking);
        }

        public class BookingRequest
        {
            public long UserId { get; set; }
            public long OutboundFlightId { get; set; }
            public long? ReturnFlightId { get; set; }
            public List<Passenger>? Passengers { get; set; }
            public double TotalAmount { get; set; }
            public string? OutboundCabinClass { get; set; }
            public string? ReturnCabinClass { get; set; }
            public string? ContactEmail { get; set; }
            public string? ContactPhone { get; set; }

            // Segment-level add-ons
            public SegmentAddOns? OutboundAddOns { get; set; }
            public SegmentAddOns? ReturnAddOns { get; set; }
        }

        public class SegmentAddOns
        {
            public string SeatPreference { get; set; }
            public string MealPreference { get; set; }
            public bool? ExtraBaggage { get; set; }
            public bool? TravelProtection { get; set; }
        }
    }
}
