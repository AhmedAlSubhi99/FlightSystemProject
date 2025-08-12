using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(FlightContext context)
        {
            context.Database.Migrate();

            if (!context.Airports.Any())
            {
                var rnd = new Random();

                // --- Airports ---
                var airports = Enumerable.Range(1, 10).Select(i =>
                    new Airport
                    {
                        IATA = $"AP{i:00}",
                        Name = $"Airport {i}",
                        City = $"City {i}",
                        Country = $"Country {i}",
                        TimeZone = "UTC+0"
                    }).ToList();
                context.Airports.AddRange(airports);
                context.SaveChanges();

                // --- Aircraft ---
                var aircrafts = Enumerable.Range(1, 10).Select(i =>
                    new Aircraft
                    {
                        TailNumber = $"TN{i:000}",
                        Model = $"Model {i}",
                        Capacity = rnd.Next(100, 301)
                    }).ToList();
                context.Aircrafts.AddRange(aircrafts);
                context.SaveChanges();

                // --- Crew Members ---
                var roles = new[] { "Pilot", "CoPilot", "FlightAttendant" };
                var crewMembers = Enumerable.Range(1, 20).Select(i =>
                    new CrewMember
                    {
                        FullName = $"Crew Member {i}",
                        Role = roles[rnd.Next(roles.Length)],
                        LicenseNo = rnd.Next(2) == 0 ? $"LIC{i:0000}" : null
                    }).ToList();
                context.CrewMembers.AddRange(crewMembers);
                context.SaveChanges();

                // --- Routes ---
                var routes = new List<Route>();
                for (int i = 1; i <= 20; i++)
                {
                    routes.Add(new Route
                    {
                        DistanceKm = rnd.Next(300, 2000),
                        OriginAirportId = airports[rnd.Next(airports.Count)].AirportId,
                        DestinationAirportId = airports[rnd.Next(airports.Count)].AirportId
                    });
                }
                context.Routes.AddRange(routes);
                context.SaveChanges();

                // --- Flights ---
                var flights = new List<Flight>();
                var statuses = new[] { "Scheduled", "Departed", "Landed" };
                var startRange = DateTime.UtcNow.AddDays(-60);
                for (int i = 1; i <= 30; i++)
                {
                    var dep = startRange.AddDays(rnd.Next(0, 60)).AddHours(rnd.Next(0, 24));
                    var arr = dep.AddHours(rnd.Next(1, 6));

                    flights.Add(new Flight
                    {
                        FlightNumber = $"FM{i:000}",
                        DepartureUtc = dep,
                        ArrivalUtc = arr,
                        Status = statuses[rnd.Next(statuses.Length)],
                        RouteId = routes[rnd.Next(routes.Count)].RouteId,
                        AircraftId = aircrafts[rnd.Next(aircrafts.Count)].AircraftId
                    });
                }
                context.Flights.AddRange(flights);
                context.SaveChanges();

                // --- Passengers ---
                var passengers = Enumerable.Range(1, 50).Select(i =>
                    new Passenger
                    {
                        FullName = $"Passenger {i}",
                        PassportNo = $"P{i:000000}",
                        Nationality = $"Country {rnd.Next(1, 6)}",
                        DOB = DateTime.UtcNow.AddYears(-rnd.Next(18, 60)).AddDays(rnd.Next(0, 365))
                    }).ToList();
                context.Passengers.AddRange(passengers);
                context.SaveChanges();

                // --- Bookings ---
                var bookings = new List<Booking>();
                for (int i = 1; i <= 100; i++)
                {
                    var bookingDate = startRange.AddDays(rnd.Next(0, 60));
                    bookings.Add(new Booking
                    {
                        BookingRef = $"BR{i:0000}",
                        BookingDate = bookingDate,
                        Status = "Confirmed",
                        PassengerId = passengers[rnd.Next(passengers.Count)].PassengerId
                    });
                }
                context.Bookings.AddRange(bookings);
                context.SaveChanges();
                // --- Tickets ---
                var tickets = new List<Ticket>();
                for (int i = 1; i <= 200; i++)
                {
                    var flight = flights[rnd.Next(flights.Count)];
                    var booking = bookings[rnd.Next(bookings.Count)];
                    var capacity = aircrafts.First(a => a.AircraftId == flight.AircraftId).Capacity;

                    tickets.Add(new Ticket
                    {
                        SeatNumber = $"{rnd.Next(1, capacity / 6)}{(char)('A' + rnd.Next(0, 6))}",
                        Fare = rnd.Next(80, 500),
                        CheckedIn = rnd.Next(2) == 0,
                        BookingId = booking.BookingId,
                        FlightId = flight.FlightId
                    });
                }

                context.Tickets.AddRange(tickets);
                context.SaveChanges();

                // --- Baggage ---
                var baggage = new List<Baggage>();
                for (int i = 1; i <= 150; i++)
                {
                    baggage.Add(new Baggage
                    {
                        TicketId = tickets[rnd.Next(tickets.Count)].TicketId,
                        WeightKg = rnd.Next(10, 45), // some over threshold
                        TagNumber = $"BAG{i:0000}"
                    });
                }
                context.Baggages.AddRange(baggage);
                context.SaveChanges();

                // --- Maintenance ---
                var maints = new List<AircraftMaintenance>();
                for (int i = 1; i <= 15; i++)
                {
                    maints.Add(new AircraftMaintenance
                    {
                        AircraftId = aircrafts[rnd.Next(aircrafts.Count)].AircraftId,
                        MaintenanceDate = DateTime.UtcNow.AddDays(-rnd.Next(0, 365)),
                        Type = "Inspection",
                        Notes = $"Routine check {i}"
                    });
                }
                context.AircraftMaintenances.AddRange(maints);
                context.SaveChanges();
            }
        }
    }
}
