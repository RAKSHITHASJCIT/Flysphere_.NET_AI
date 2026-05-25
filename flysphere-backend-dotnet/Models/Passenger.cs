using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlysphereBackendDotnet.Models
{
    [Table("passengers")]
    public class Passenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Title { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        public int? Age { get; set; }

        [Column("dob")]
        public string Dob { get; set; }

        public string Type { get; set; }

        [Column("outbound_seat")]
        public string OutboundSeat { get; set; }

        [Column("outbound_meal")]
        public string OutboundMeal { get; set; }

        [Column("return_seat")]
        public string ReturnSeat { get; set; }

        [Column("return_meal")]
        public string ReturnMeal { get; set; }

        public bool? Baggage { get; set; }

        [Column("booking_id")]
        public long? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}
