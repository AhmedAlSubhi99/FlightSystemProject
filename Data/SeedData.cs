using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(FlightContext context)
        {
            // Apply migrations and seed idempotently (no early return)
            context.Database.Migrate();

            var rnd = new Random(971);
            var today = DateTime.UtcNow.Date;

            // -------------------- Airports
            if (!context.Airports.Any())
            {
                var airportsToAdd = new List<Airport>
                {
                    new(){ IATA="DXB", Name="Dubai Intl",            City="Dubai",        Country="United Arab Emirates", TimeZone="Asia/Dubai"},
                    new(){ IATA="AUH", Name="Abu Dhabi Intl",        City="Abu Dhabi",    Country="United Arab Emirates", TimeZone="Asia/Dubai"},
                    new(){ IATA="SHJ", Name="Sharjah Intl",          City="Sharjah",      Country="United Arab Emirates", TimeZone="Asia/Dubai"},
                    new(){ IATA="DOH", Name="Hamad Intl",            City="Doha",         Country="Qatar",               TimeZone="Asia/Doha"},
                    new(){ IATA="BAH", Name="Bahrain Intl",          City="Manama",       Country="Bahrain",             TimeZone="Asia/Bahrain"},
                    new(){ IATA="KWI", Name="Kuwait Intl",           City="Kuwait City",  Country="Kuwait",              TimeZone="Asia/Kuwait"},
                    new(){ IATA="RUH", Name="King Khalid Intl",      City="Riyadh",       Country="Saudi Arabia",        TimeZone="Asia/Riyadh"},
                    new(){ IATA="JED", Name="King Abdulaziz Intl",   City="Jeddah",       Country="Saudi Arabia",        TimeZone="Asia/Riyadh"},
                    new(){ IATA="DMM", Name="King Fahd Intl",        City="Dammam",       Country="Saudi Arabia",        TimeZone="Asia/Riyadh"},
                    new(){ IATA="MCT", Name="Muscat Intl",           City="Muscat",       Country="Oman",                TimeZone="Asia/Muscat"},
                    new(){ IATA="SLL", Name="Salalah",               City="Salalah",      Country="Oman",                TimeZone="Asia/Muscat"},
                };
                context.Airports.AddRange(airportsToAdd);
                context.SaveChanges();
            }
            var airports = context.Airports.AsNoTracking().ToList();

            // -------------------- Aircraft
            if (!context.Aircrafts.Any())
            {
                string[] models = { "A320", "A321neo", "B737-8", "A330-300", "B787-9" };
                var aircraftToAdd = Enumerable.Range(1, 10).Select(i => new Aircraft
                {
                    TailNumber = $"A9-{i:000}",
                    Model = models[i % models.Length],
                    Capacity = 120 + (i % 10) * 20
                }).ToList();
                context.Aircrafts.AddRange(aircraftToAdd);
                context.SaveChanges();
            }
            var aircraft = context.Aircrafts.AsNoTracking().ToList();

            // -------------------- Crew
            if (!context.CrewMembers.Any())
            {
                var roles = new[] { CrewRole.Pilot, CrewRole.CoPilot, CrewRole.FlightAttendant, CrewRole.FlightAttendant };
                var crewToAdd = Enumerable.Range(1, 20).Select(i => new CrewMember
                {
                    FullName = $"Crew Member {i}",
                    Role = roles[i % roles.Length],
                    LicenseNo = (i % 3 == 0) ? $"LIC{i:000000}" : null
                }).ToList();
                context.CrewMembers.AddRange(crewToAdd);
                context.SaveChanges();
            }
            var crew = context.CrewMembers.AsNoTracking().ToList();

            // -------------------- Routes
            if (!context.Routes.Any())
            {
                int RandDistance() => 300 + (rnd.Next(0, 12) * 100);
                var routesToAdd = new List<Route>();
                var nAir = airports.Count;

                for (int i = 0; i < 20; i++)
                {
                    var o = airports[i % nAir].AirportId;
                    var d = airports[(i * 3 + 1) % nAir].AirportId;
                    if (o == d) d = airports[(i + 2) % nAir].AirportId;

                    routesToAdd.Add(new Route
                    {
                        OriginAirportId = o,
                        DestinationAirportId = d,
                        DistanceKm = RandDistance()
                    });
                }
                context.Routes.AddRange(routesToAdd);
                context.SaveChanges();
            }
            var routes = context.Routes.AsNoTracking().ToList();

            // -------------------- Flights
            if (!context.Flights.Any())
            {
                var fStatuses = new[] { FlightStatus.Scheduled, FlightStatus.Departed, FlightStatus.Landed, FlightStatus.Delayed };
                var start = today.AddDays(-30);
                var flightsToAdd = new List<Flight>();

                for (int i = 1; i <= 30; i++)
                {
                    var route = routes[i % routes.Count];
                    var dep = start.AddDays(i).AddHours(6 + (i % 12)); // spread during day
                    var arr = dep.AddHours(2 + (i % 4));               // 2–5h legs

                    flightsToAdd.Add(new Flight
                    {
                        FlightNumber = $"GF{i:000}",
                        DepartureUtc = dep,
                        ArrivalUtc = arr,
                        Status = fStatuses[i % fStatuses.Length],
                        RouteId = route.RouteId,
                        AircraftId = aircraft[i % aircraft.Count].AircraftId
                    });
                }
                context.Flights.AddRange(flightsToAdd);
                context.SaveChanges();
            }

            // Ensure at least one flight **today** (UTC)
            if (!context.Flights.Any(f => f.DepartureUtc >= today && f.DepartureUtc < today.AddDays(1)))
            {
                var route = routes.First();
                var ac = aircraft.First();
                context.Flights.Add(new Flight
                {
                    FlightNumber = "GF999",
                    DepartureUtc = today.AddHours(9),
                    ArrivalUtc = today.AddHours(12),
                    Status = FlightStatus.Scheduled,
                    RouteId = route.RouteId,
                    AircraftId = ac.AircraftId
                });
                context.SaveChanges();
            }
            var flights = context.Flights.AsNoTracking().ToList();

            // -------------------- Passengers
            if (!context.Passengers.Any())
            {
                string[] nationalities = { "United Arab Emirates", "Saudi Arabia", "Qatar", "Bahrain", "Kuwait",
                                           "Oman", "India", "Pakistan", "Philippines", "Egypt", "United Kingdom" };
                var paxToAdd = Enumerable.Range(1, 50).Select(i => new Passenger
                {
                    FullName = $"Passenger {i}",
                    PassportNo = $"GCC{i:0000000}",
                    Nationality = nationalities[i % nationalities.Length],
                    DOB = today.AddYears(-(20 + (i % 35))).AddDays(i % 365)
                }).ToList();
                context.Passengers.AddRange(paxToAdd);
                context.SaveChanges();
            }
            var pax = context.Passengers.AsNoTracking().ToList();

            // -------------------- Bookings
            if (!context.Bookings.Any())
            {
                var bStatuses = new[] { BookingStatus.Pending, BookingStatus.Confirmed, BookingStatus.Canceled, BookingStatus.Completed };
                var start = today.AddDays(-30);

                var bookingsToAdd = new List<Booking>();
                for (int i = 1; i <= 100; i++)
                {
                    bookingsToAdd.Add(new Booking
                    {
                        BookingRef = $"BR{i:00000000}", // 10 chars (BR + 8 digits)
                        BookingDate = start.AddDays(i % 50).AddHours(i % 24),
                        Status = bStatuses[i % bStatuses.Length],
                        PassengerId = pax[i % pax.Count].PassengerId
                    });
                }
                context.Bookings.AddRange(bookingsToAdd);
                context.SaveChanges();
            }
            var bookings = context.Bookings.AsNoTracking().ToList();

            // -------------------- Tickets
            if (!context.Tickets.Any())
            {
                // Build seat map per flight according to aircraft capacity
                var acById = aircraft.ToDictionary(a => a.AircraftId, a => a);
                var seatMapByFlight = flights.ToDictionary(
                    f => f.FlightId,
                    f => BuildSeatMap(acById[f.AircraftId].Capacity));

                var ticketsToAdd = new List<Ticket>();
                for (int i = 1; i <= 200; i++)
                {
                    var f = flights[(i * 5) % flights.Count];
                    var seatList = seatMapByFlight[f.FlightId];
                    if (seatList.Count == 0) continue; // capacity exhausted

                    var seat = seatList[0]; seatList.RemoveAt(0);

                    ticketsToAdd.Add(new Ticket
                    {
                        SeatNumber = seat,
                        Fare = 200 + (i % 18) * 15m,   // 200..455
                        CheckedIn = (i % 6 == 0),
                        FlightId = f.FlightId,
                        BookingId = bookings[(i * 3) % bookings.Count].BookingId
                    });
                }
                context.Tickets.AddRange(ticketsToAdd);
                context.SaveChanges();
            }
            var tickets = context.Tickets.AsNoTracking().Select(t => t.TicketId).ToList();

            // -------------------- Baggage  (for ALL tickets, not just 150)
            if (!context.Baggage.Any())
            {
                var bags = new List<Baggage>();
                foreach (var tid in tickets)
                {
                    int bagCount = rnd.Next(1, 3); // 1–2 bags
                    for (int b = 1; b <= bagCount; b++)
                    {
                        var w = 12m + (decimal)rnd.NextDouble() * 18m; // 12.0–30.0
                        bags.Add(new Baggage
                        {
                            TicketId = tid,
                            WeightKg = Math.Round(w, 1),
                            TagNumber = $"TAG{tid:000000}-{b}"
                        });
                    }
                }
                context.Baggage.AddRange(bags);
                context.SaveChanges();
            }

            // -------------------- FlightCrew
            if (!context.FlightCrews.Any())
            {
                var fcs = new List<FlightCrew>();
                foreach (var f in flights)
                {
                    var assigned = crew.OrderBy(_ => rnd.Next()).Take(2 + rnd.Next(0, 4)).ToList();
                    foreach (var c in assigned)
                    {
                        fcs.Add(new FlightCrew
                        {
                            FlightId = f.FlightId,
                            CrewId = c.CrewId,
                            RoleOnFlight = c.Role.ToString()
                        });
                    }
                }
                context.FlightCrews.AddRange(fcs);
                context.SaveChanges();
            }

            // -------------------- Maintenance
            if (!context.AircraftMaintenances.Any())
            {
                var maint = new List<AircraftMaintenance>();
                for (int i = 1; i <= 15; i++)
                {
                    maint.Add(new AircraftMaintenance
                    {
                        AircraftId = aircraft[(i * 2) % aircraft.Count].AircraftId,
                        MaintenanceDate = today.AddDays(-rnd.Next(5, 120)),
                        Type = (i % 3 == 0) ? "A-Check" : (i % 3 == 1) ? "B-Check" : "Inspection",
                        Notes = $"Routine maintenance #{i}"
                    });
                }
                context.AircraftMaintenances.AddRange(maint);
                context.SaveChanges();
            }
        }

        // Build seat labels up to capacity, 6 seats per row (A..F)
        private static List<string> BuildSeatMap(int capacity)
        {
            var seats = new List<string>(capacity);
            if (capacity <= 0) return seats;
            const int perRow = 6;
            int rows = (int)Math.Ceiling(capacity / (double)perRow);
            for (int r = 1; r <= rows; r++)
            {
                for (int s = 0; s < perRow; s++)
                {
                    if (seats.Count >= capacity) break;
                    seats.Add($"{r}{(char)('A' + s)}");
                }
            }
            return seats;
        }

    }
}
