using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options) { }

        // DbSets for each entity
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<FlightCrew> FlightCrews { get; set; }
        public DbSet<Baggage> Baggage { get; set; }
        public DbSet<AircraftMaintenance> AircraftMaintenances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=FlightDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Unique Constraints ---
            modelBuilder.Entity<Airport>()
                .HasIndex(a => a.IATA)
                .IsUnique();

            modelBuilder.Entity<Aircraft>()
                .HasIndex(a => a.TailNumber)
                .IsUnique();

            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.PassportNo)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingRef)
                .IsUnique();

            // Composite unique constraint for FlightNumber + Departure date
            modelBuilder.Entity<Flight>()
                .HasIndex(f => new { f.FlightNumber, DepartureDate = f.DepartureUtc.Date })
                .IsUnique();

            // --- Relationships ---
            modelBuilder.Entity<Route>()
                .HasOne(r => r.OriginAirport)
                .WithMany(a => a.OriginRoutes)
                .HasForeignKey(r => r.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.DestinationAirport)
                .WithMany(a => a.DestinationRoutes)
                .HasForeignKey(r => r.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightCrew>()
                .HasKey(fc => new { fc.FlightId, fc.CrewId });

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Flight)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FlightId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Baggage>()
                .HasOne(b => b.Ticket)
                .WithMany(t => t.BaggageItems)
                .HasForeignKey(b => b.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AircraftMaintenance>()
                .HasOne(am => am.Aircraft)
                .WithMany(a => a.Maintenances)
                .HasForeignKey(am => am.AircraftId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
