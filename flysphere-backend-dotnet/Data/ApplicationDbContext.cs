using Microsoft.EntityFrameworkCore;
using FlysphereBackendDotnet.Models;

namespace FlysphereBackendDotnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSegment> BookingSegments { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("users");
            // ✅ Explicitly map Flight entity to correct PostgreSQL table
            modelBuilder.Entity<Flight>().ToTable("flightmgtable");
            modelBuilder.Entity<Booking>().ToTable("bookings");
            modelBuilder.Entity<BookingSegment>().ToTable("booking_segments");
            modelBuilder.Entity<Passenger>().ToTable("passengers");

            // ✅ Force explicit column mappings for BookingSegment (fix case-sensitivity)
            modelBuilder.Entity<BookingSegment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SegmentNo).HasColumnName("segment_no");
                entity.Property(e => e.CabinType).HasColumnName("cabin_type");
                entity.Property(e => e.SeatPreference).HasColumnName("seat_preference");
                entity.Property(e => e.MealPreference).HasColumnName("meal_preference");
                entity.Property(e => e.ExtraBaggage).HasColumnName("extra_baggage");
                entity.Property(e => e.TravelProtection).HasColumnName("travel_protection");
                entity.Property(e => e.BookingId).HasColumnName("booking_id");
                entity.Property(e => e.FlightId).HasColumnName("flight_id");
            });

            // ✅ Force explicit column mappings for Passenger (fix case-sensitivity)
            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.BookingId).HasColumnName("booking_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.Dob).HasColumnName("dob");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Age).HasColumnName("age");
                entity.Property(e => e.Baggage).HasColumnName("baggage");
                entity.Property(e => e.OutboundMeal).HasColumnName("outbound_meal");
                entity.Property(e => e.OutboundSeat).HasColumnName("outbound_seat");
                entity.Property(e => e.ReturnMeal).HasColumnName("return_meal");
                entity.Property(e => e.ReturnSeat).HasColumnName("return_seat");
            });

            // ✅ Fix PostgreSQL timestamp with time zone issue
            modelBuilder.Entity<Flight>()
                .Property(f => f.DepartureDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Flight>()
                .Property(f => f.ArrivalDate)
                .HasColumnType("timestamp without time zone");
        }
    }
}
