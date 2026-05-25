using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlysphereBackendDotnet.Models
{
    [Table("booking_segments")]
    public class BookingSegment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("segment_no")]
        public int? SegmentNo { get; set; }

        [Column("cabin_type")]
        public string CabinType { get; set; }

        [Column("seat_preference")]
        public string SeatPreference { get; set; }

        [Column("meal_preference")]
        public string MealPreference { get; set; }

        [Column("extra_baggage")]
        public bool? ExtraBaggage { get; set; }

        [Column("travel_protection")]
        public bool? TravelProtection { get; set; }

        [Required]
        [Column("booking_id")]
        public long BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [Column("flight_id")]
        public long? FlightId { get; set; }

        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
    }
}
