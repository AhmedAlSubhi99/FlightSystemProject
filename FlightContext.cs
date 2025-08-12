using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options) { }

        // DbSets
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<FlightCrew> FlightCrews { get; set; }
        public DbSet<Baggage> Baggages { get; set; }
        public DbSet<AircraftMaintenance> AircraftMaintenances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not already done (so DI can still pass options)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=localhost;Database=FlightDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== AIRPORT =====
            modelBuilder.Entity<Airport>(e =>
            {
                e.HasKey(a => a.AirportId);
                e.HasIndex(a => a.IATA).IsUnique();
                e.Property(a => a.IATA).IsRequired().HasMaxLength(3);
                e.Property(a => a.Name).IsRequired().HasMaxLength(120);
                e.Property(a => a.City).IsRequired().HasMaxLength(80);
                e.Property(a => a.Country).IsRequired().HasMaxLength(80);
                e.Property(a => a.TimeZone).IsRequired().HasMaxLength(64);
            });

            // ===== AIRCRAFT =====
            modelBuilder.Entity<Aircraft>(e =>
            {
                e.HasKey(a => a.AircraftId);
                e.HasIndex(a => a.TailNumber).IsUnique();
                e.Property(a => a.TailNumber).IsRequired().HasMaxLength(16);
                e.Property(a => a.Model).IsRequired().HasMaxLength(80);
                e.Property(a => a.Capacity).IsRequired();
            });

            // ===== CREWMEMBER =====
            modelBuilder.Entity<CrewMember>(e =>
            {
                e.HasKey(c => c.CrewId);
                e.Property(c => c.FullName).IsRequired().HasMaxLength(120);
                e.Property(c => c.Role).IsRequired().HasMaxLength(30);
                e.Property(c => c.LicenseNo).HasMaxLength(40); // Nullable by model
            });

            // ===== ROUTE =====
            modelBuilder.Entity<Route>(e =>
            {
                e.HasKey(r => r.RouteId);
                e.Property(r => r.DistanceKm).IsRequired();

                e.HasOne(r => r.OriginAirport)
                 .WithMany(a => a.OriginRoutes)
                 .HasForeignKey(r => r.OriginAirportId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.DestinationAirport)
                 .WithMany(a => a.DestinationRoutes)
                 .HasForeignKey(r => r.DestinationAirportId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== FLIGHT =====
            modelBuilder.Entity<Flight>(e =>
            {
                e.HasKey(f => f.FlightId);

                // Computed DATE column from DepartureUtc
                e.Property<DateTime>("DepartureDate")
                 .HasColumnType("date")
                 .HasComputedColumnSql("CAST([DepartureUtc] AS date)", stored: true);

                // Unique index on FlightNumber + computed date
                e.HasIndex(new[] { nameof(Flight.FlightNumber), "DepartureDate" })
                 .IsUnique();

                e.Property(f => f.FlightNumber).IsRequired().HasMaxLength(12);
                e.Property(f => f.DepartureUtc).IsRequired();
                e.Property(f => f.ArrivalUtc).IsRequired();
                e.Property(f => f.Status).IsRequired().HasMaxLength(20);

                e.HasOne(f => f.Route)
                 .WithMany(r => r.Flights)
                 .HasForeignKey(f => f.RouteId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(f => f.Aircraft)
                 .WithMany(a => a.Flights)
                 .HasForeignKey(f => f.AircraftId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== PASSENGER =====
            modelBuilder.Entity<Passenger>(e =>
            {
                e.HasKey(p => p.PassengerId);
                e.HasIndex(p => p.PassportNo).IsUnique();
                e.Property(p => p.FullName).IsRequired().HasMaxLength(120);
                e.Property(p => p.PassportNo).IsRequired().HasMaxLength(20);
                e.Property(p => p.Nationality).IsRequired().HasMaxLength(50);
                e.Property(p => p.DOB).IsRequired();
            });

            // ===== BOOKING =====
            modelBuilder.Entity<Booking>(e =>
            {
                e.HasKey(b => b.BookingId);
                e.HasIndex(b => b.BookingRef).IsUnique();
                e.Property(b => b.BookingRef).IsRequired().HasMaxLength(16);
                e.Property(b => b.BookingDate).IsRequired();
                e.Property(b => b.Status).IsRequired().HasMaxLength(20);

                e.HasOne(b => b.Passenger)
                 .WithMany(p => p.Bookings)
                 .HasForeignKey(b => b.PassengerId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== TICKET =====
            modelBuilder.Entity<Ticket>(e =>
            {
                e.HasKey(t => t.TicketId);
                e.Property(t => t.SeatNumber).IsRequired().HasMaxLength(6);
                e.Property(t => t.Fare).IsRequired().HasColumnType("decimal(10,2)");
                e.Property(t => t.CheckedIn).IsRequired();

                e.HasOne(t => t.Booking)
                 .WithMany(b => b.Tickets)
                 .HasForeignKey(t => t.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(t => t.Flight)
                 .WithMany(f => f.Tickets)
                 .HasForeignKey(t => t.FlightId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== FLIGHTCREW =====
            modelBuilder.Entity<FlightCrew>(e =>
            {
                e.HasKey(fc => new { fc.FlightId, fc.CrewId });
                e.Property(fc => fc.RoleOnFlight).IsRequired().HasMaxLength(30);
            });

            // ===== BAGGAGE =====
            modelBuilder.Entity<Baggage>(e =>
            {
                e.HasKey(b => b.BaggageId);
                e.Property(b => b.WeightKg).IsRequired().HasColumnType("decimal(10,2)");
                e.Property(b => b.TagNumber).IsRequired().HasMaxLength(20);

                e.HasOne(b => b.Ticket)
                 .WithMany(t => t.BaggageItems)
                 .HasForeignKey(b => b.TicketId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== AIRCRAFT MAINTENANCE =====
            modelBuilder.Entity<AircraftMaintenance>(e =>
            {
                e.HasKey(m => m.MaintenanceId);
                e.Property(m => m.MaintenanceDate).IsRequired();
                e.Property(m => m.Type).IsRequired().HasMaxLength(40);
                e.Property(m => m.Notes).HasMaxLength(200);

                e.HasOne(m => m.Aircraft)
                 .WithMany(a => a.Maintenances)
                 .HasForeignKey(m => m.AircraftId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
