using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlysphereBackendDotnet.Models
{
    [Table("bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("booking_id")]
        public string BookingId { get; set; }

        [Column("user_id")]
        public long? UserId { get; set; }

        [Required]
        [Column("flight_id")]
        public long FlightId { get; set; }

        [Column("total_amount")]
        public double? TotalAmount { get; set; }

        [Column("contact_email")]
        public string? ContactEmail { get; set; }

        [Column("contact_phone")]
        public string? ContactPhone { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public List<BookingSegment> Segments { get; set; }

        public List<Passenger> Passengers { get; set; }
    }
}
