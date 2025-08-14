using System;
using Microsoft.EntityFrameworkCore;
using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.Repositories;
using FlightSystemUsingAPI.Service;

namespace FlightSystemUsingAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // --- DB ---
            var options = new DbContextOptionsBuilder<FlightContext>()
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FlightDBase;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

            using var ctx = new FlightContext(options);

            // Seed Gulf data (idempotent)
            SeedData.Initialize(ctx);
            Console.WriteLine($"[SEED] Airports:   {ctx.Airports.Count()}");
            Console.WriteLine($"[SEED] Aircrafts:  {ctx.Aircrafts.Count()}");
            Console.WriteLine($"[SEED] Crew:       {ctx.CrewMembers.Count()}");
            Console.WriteLine($"[SEED] Routes:     {ctx.Routes.Count()}");
            Console.WriteLine($"[SEED] Flights:    {ctx.Flights.Count()}");
            Console.WriteLine($"[SEED] Passengers: {ctx.Passengers.Count()}");
            Console.WriteLine($"[SEED] Bookings:   {ctx.Bookings.Count()}");
            Console.WriteLine($"[SEED] Tickets:    {ctx.Tickets.Count()}");
            Console.WriteLine($"[SEED] Baggage:    {ctx.Baggage.Count()}");
            Console.WriteLine(ctx.Database.GetDbConnection().ConnectionString);
            Console.WriteLine($"Counts: Airports={ctx.Airports.Count()}, Flights={ctx.Flights.Count()}, Tickets={ctx.Tickets.Count()}, Baggage={ctx.Baggage.Count()}");

            // --- Repositories ---
            var airportRepo = new AirportRepository(ctx);
            var routeRepo = new RouteRepository(ctx);
            var aircraftRepo = new AircraftRepository(ctx);
            var crewRepo = new CrewRepository(ctx);
            var flightRepo = new FlightRepository(ctx);
            var passengerRepo = new PassengerRepository(ctx);
            var bookingRepo = new BookingRepository(ctx);
            var ticketRepo = new TicketRepository(ctx);
            var baggageRepo = new BaggageRepository(ctx);
            var maintenanceRepo = new MaintenanceRepository(ctx);

            // --- Service ---
            var svc = new FlightService(
                flightRepo, routeRepo, airportRepo, aircraftRepo,
                ticketRepo, bookingRepo, crewRepo, passengerRepo, baggageRepo, maintenanceRepo);

            // --- Run console menu (all handlers are in /Menu/AppMenu.cs) ---
            Menu.AppMenu.Run(svc);
        }
    }
}
