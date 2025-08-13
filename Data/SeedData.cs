using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Data
{
    public static class SeedData
    {
        // call from Program.cs: await SeedData.InitializeAsync(context);
        public static async Task InitializeAsync(FlightContext context)
        {
            await context.Database.MigrateAsync();

            // already seeded? bail out
            if (await context.Airports.AnyAsync()) return;

            var rnd = new Random(42);

            // ---------------- AIRPORTS (10) ----------------

            var iata = new[] { "MCT", "DXB", "LHR", "JFK", "CDG", "FRA", "DOH", "BOM", "SIN", "SYD" };
            var airports = iata.Select((code, i) => new Airport
            {
                IATA = code,                                                // len 3
                Name = $"Airport {i + 1}",                                    // <=120
                City = $"City {i + 1}",                                       // <=80
                Country = $"Country {i + 1}",                                  // <=80
                TimeZone = "UTC+0"                                          // <=64
            }).ToList();
            context.Airports.AddRange(airports);
            await context.SaveChangesAsync();

            // ---------------- AIRCRAFT (10) ----------------
          
            var aircrafts = Enumerable.Range(1, 10).Select(i => new Aircraft
            {
                TailNumber = $"TN{i:000}",         // max 6 chars
                Model = $"Model {i}",              // short
                Capacity = 100 + (i * 10)          // 110..200
            }).ToList();
            context.Aircrafts.AddRange(aircrafts);
            await context.SaveChangesAsync();

            // ---------------- CREW MEMBERS (20) ----------------
            var roles = new[] { CrewRole.Pilot, CrewRole.CoPilot, CrewRole.FlightAttendant };
            var crewMembers = Enumerable.Range(1, 20).Select(i => new CrewMember
            {
                FullName = $"Crew Member {i}",
                Role = roles[i % roles.Length],          // enum value
                LicenseNo = (i % 3 == 0) ? $"LIC{i:000000}" : null
            }).ToList();
            context.CrewMembers.AddRange(crewMembers);
            await context.SaveChangesAsync();

            // ---------------- ROUTES (20) ----------------
            // Ensure origin != destination
            var routes = new List<Route>();
            for (int i = 0; i < 20; i++)
            {
                int originIdx = i % airports.Count;
                int destIdx = (i + 1) % airports.Count;
                routes.Add(new Route
                {
                    OriginAirportId = airports[originIdx].AirportId,
                    DestinationAirportId = airports[destIdx].AirportId,
                    DistanceKm = 400 + (i * 50)
                });
            }
            context.Routes.AddRange(routes);
            await context.SaveChangesAsync();

            // ---------------- FLIGHTS (30) ----------------
            var flightStatuses = new[] { FlightStatus.Scheduled, FlightStatus.Departed, FlightStatus.Landed };
            var flights = new List<Flight>();
            var start = DateTime.UtcNow.Date.AddDays(-45);
            for (int i = 1; i <= 30; i++)
            {
                var dep = start.AddDays(i);
                var arr = dep.AddHours(2 + (i % 4));

                flights.Add(new Flight
                {
                    FlightNumber = $"FM{i:000}",
                    DepartureUtc = dep.AddHours(8 + (i % 6)),
                    ArrivalUtc = arr.AddHours(8 + (i % 6)),
                    Status = flightStatuses[i % flightStatuses.Length],   // enum 
                    RouteId = routes[i % routes.Count].RouteId,            // IDs valid only if saved
                    AircraftId = aircrafts[i % aircrafts.Count].AircraftId
                });
            }
            context.Flights.AddRange(flights);
            await context.SaveChangesAsync();

            // ---------------- PASSENGERS (50) ----------------
            // Unique PassportNo (<=20), names short
            var passengers = Enumerable.Range(1, 50).Select(i => new Passenger
            {
                FullName = $"Passenger {i}",          // <=120
                PassportNo = $"P{i:000000}",          // <=20, unique
                Nationality = $"Nat{i % 10}",           // <=50
                DOB = DateTime.UtcNow.AddYears(-20 - (i % 30)).AddDays(i)
            }).ToList();
            context.Passengers.AddRange(passengers);
            await context.SaveChangesAsync();

            // ---------------- BOOKINGS (100) ----------------
            // Unique BookingRef (<=16), Status (<=20)
            var bookings = new List<Booking>();
            for (int i = 1; i <= 100; i++)
            {
                bookings.Add(new Booking
                {
                    BookingRef = $"BR{i:0000000000}".Substring(0, 10), // exactly 10 chars, unique
                    BookingDate = start.AddDays(i % 40).AddHours(i % 24),
                    Status = BookingStatus.Confirmed,
                    PassengerId = passengers[i % passengers.Count].PassengerId
                });
            }
            context.Bookings.AddRange(bookings);
            await context.SaveChangesAsync();

            // ---------------- TICKETS (200) ----------------
            // SeatNumber (<=6), Fare decimal(10,2)
            var tickets = new List<Ticket>();
            for (int i = 1; i <= 200; i++)
            {
                var flight = flights[i % flights.Count];
                var aircraft = aircrafts.First(a => a.AircraftId == flight.AircraftId);
                int rows = Math.Max(1, aircraft.Capacity / 6); // 6 seats per row (A..F)
                var seat = $"{rnd.Next(1, rows + 1)}{(char)('A' + rnd.Next(0, 6))}";

                tickets.Add(new Ticket
                {
                    SeatNumber = seat,                                // <=6
                    Fare = Math.Round(80 + (decimal)rnd.NextDouble() * 420, 2),
                    CheckedIn = (i % 2 == 0),
                    BookingId = bookings[i % bookings.Count].BookingId,
                    FlightId = flight.FlightId
                });
            }
            context.Tickets.AddRange(tickets);
            await context.SaveChangesAsync();

            // ---------------- BAGGAGE (150) ----------------
            // TagNumber (<=20), WeightKg decimal(10,2)
            var baggage = new List<Baggage>();
            for (int i = 1; i <= 150; i++)
            {
                baggage.Add(new Baggage
                {
                    TicketId = tickets[i % tickets.Count].TicketId,
                    WeightKg = Math.Round(10 + (decimal)rnd.NextDouble() * 35, 2), // 10..45 kg
                    TagNumber = $"BAG{i:000000}"                                    // <=20
                });
            }
            context.Baggages.AddRange(baggage);
            await context.SaveChangesAsync();

            // ---------------- FLIGHT CREW ASSIGNMENTS (~60) ----------------
            // composite PK (FlightId, CrewId), RoleOnFlight <=30
            var flightCrews = new List<FlightCrew>();
            foreach (var f in flights)
            {
                // pick 2–5 crew
                var assigned = crewMembers
                    .OrderBy(_ => rnd.Next())
                    .Take(2 + rnd.Next(0, 4))
                    .ToList();

                foreach (var crew in assigned)  
                {
                    flightCrews.Add(new FlightCrew
                    {
                        FlightId = f.FlightId,
                        CrewId = crew.CrewId,
                        RoleOnFlight = crew.Role.ToString() 
                    });
                }
            }
            context.FlightCrews.AddRange(flightCrews);
            await context.SaveChangesAsync();

            // ---------------- MAINTENANCE (15) ----------------
            // Type <=40, Notes <=200
            var maints = new List<AircraftMaintenance>();
            for (int i = 1; i <= 15; i++)
            {
                maints.Add(new AircraftMaintenance
                {
                    AircraftId = aircrafts[i % aircrafts.Count].AircraftId,
                    MaintenanceDate = DateTime.UtcNow.AddDays(-rnd.Next(10, 300)),
                    Type = (i % 3 == 0) ? "Inspection" : (i % 3 == 1) ? "A-Check" : "B-Check",
                    Notes = $"Routine maintenance #{i}"
                });
            }
            context.AircraftMaintenances.AddRange(maints);
            await context.SaveChangesAsync();
        }
    }
}
