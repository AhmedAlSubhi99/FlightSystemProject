using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Data
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options) { }

        // If you're NOT using DI, keep this so the console app still runs:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Local SQL Server (change as needed)
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=FlightDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        // DbSets
        public DbSet<Airport> Airports => Set<Airport>();
        public DbSet<Route> Routes => Set<Route>();
        public DbSet<Aircraft> Aircrafts => Set<Aircraft>();
        public DbSet<AircraftMaintenance> AircraftMaintenances => Set<AircraftMaintenance>();
        public DbSet<CrewMember> CrewMembers => Set<CrewMember>();
        public DbSet<Flight> Flights => Set<Flight>();
        public DbSet<FlightCrew> FlightCrews => Set<FlightCrew>();
        public DbSet<Passenger> Passengers => Set<Passenger>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Baggage> Baggage => Set<Baggage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------------- Airports
            modelBuilder.Entity<Airport>()
                .HasIndex(a => a.IATA)
                .IsUnique();

            // ---------------- Routes (two FKs to Airport; restrict delete to avoid cycles)
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

            // ---------------- Aircraft
            modelBuilder.Entity<Aircraft>()
                .HasIndex(a => a.TailNumber)
                .IsUnique();

            // ---------------- Passengers
            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.PassportNo)
                .IsUnique();

            // ---------------- Bookings
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingRef)
                .IsUnique();

            // ---------------- Flights
            // Unique by (FlightNumber, DepartureUtc) – if you want "per day" uniqueness,
            // this is a good practical approximation without a computed column.
            modelBuilder.Entity<Flight>()
                .HasIndex(f => new { f.FlightNumber, f.DepartureUtc })
                .IsUnique();

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Route)
                .WithMany(r => r.Flights)
                .HasForeignKey(f => f.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AircraftId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------- FlightCrew (composite PK)
            modelBuilder.Entity<FlightCrew>()
                .HasKey(fc => new { fc.FlightId, fc.CrewId });

            modelBuilder.Entity<FlightCrew>()
                .HasOne(fc => fc.Flight)
                .WithMany(f => f.FlightCrews)
                .HasForeignKey(fc => fc.FlightId);

            modelBuilder.Entity<FlightCrew>()
                .HasOne(fc => fc.CrewMember)
                .WithMany(c => c.FlightCrews)
                .HasForeignKey(fc => fc.CrewId);

            modelBuilder.Entity<FlightCrew>()
                .Property(fc => fc.RoleOnFlight)
                .HasMaxLength(50)
                .IsRequired();

            // ---------------- Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Flight)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FlightId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId);

            // Ensure a seat is unique per flight
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.FlightId, t.SeatNumber })
                .IsUnique();

            // Decimal precision (in case attributes are missing)
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Fare)
                .HasPrecision(10, 2);

            // ---------------- Baggage
            modelBuilder.Entity<Baggage>()
                .HasOne(b => b.Ticket)
                .WithMany(t => t.BaggageItems) // or .Baggage if you used that name
                .HasForeignKey(b => b.TicketId);

            modelBuilder.Entity<Baggage>()
                .HasIndex(b => b.TagNumber)
                .IsUnique();

            modelBuilder.Entity<Baggage>()
                .Property(b => b.WeightKg)
                .HasPrecision(6, 2);

            // ---------------- AircraftMaintenance
            modelBuilder.Entity<AircraftMaintenance>()
                .HasOne(m => m.Aircraft)
                .WithMany(a => a.Maintenances)
                .HasForeignKey(m => m.AircraftId);

            modelBuilder.Entity<AircraftMaintenance>()
                .Property(m => m.Type)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AircraftMaintenance>()
                .Property(m => m.Notes)
                .HasMaxLength(1000);
        }
    }
}
