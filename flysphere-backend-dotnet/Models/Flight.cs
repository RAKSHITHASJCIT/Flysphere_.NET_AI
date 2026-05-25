using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlysphereBackendDotnet.Models
{
    [Table("flightmgtable")]
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("flightid")]
        public long Id { get; set; }

        [Column("airlinename")]
        public string AirlineName { get; set; }

        [Column("flighttype")]
        public string FlightType { get; set; }

        [Column("departureairport")]
        public string DepartureAirport { get; set; }

        [Column("arrivalairport")]
        public string ArrivalAirport { get; set; }

        [Column("departuredate")]
        public DateTime? DepartureDate { get; set; }

        [Column("arrivaldate")]
        public DateTime? ArrivalDate { get; set; }

        [Column("departuretime")]
        public TimeSpan? DepartureTime { get; set; }

        [Column("arrivaltime")]
        public TimeSpan? ArrivalTime { get; set; }

        [Column("flightno")]
        public string FlightNo { get; set; }

        [Column("flightstatus")]
        public string FlightStatus { get; set; }

        [Column("totaleconomyseats")]
        public int? TotalEconomySeats { get; set; }

        [Column("totalbusinessseats")]
        public int? TotalBusinessSeats { get; set; }

        [Column("totalfirstclassseats")]
        public int? TotalFirstClassSeats { get; set; }

        [Column("economyadultfare")]
        public int? EconomyAdultFare { get; set; }

        [Column("economychildfare")]
        public int? EconomyChildFare { get; set; }

        [Column("businessadultfare")]
        public int? BusinessAdultFare { get; set; }

        [Column("businesschildfare")]
        public int? BusinessChildFare { get; set; }

        [Column("firstadultfare")]
        public int? FirstAdultFare { get; set; }

        [Column("firstchildfare")]
        public int? FirstChildFare { get; set; }

        [Column("aircraft_type")]
        public string? AircraftType { get; set; }
    }
}
