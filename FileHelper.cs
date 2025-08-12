using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.FileHandling
{
    public class FileSaveLoad
    {
        private readonly string _folderPath;

        public FileSaveLoad(string folderPath = "DataFiles")
        {
            _folderPath = folderPath;
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
        }

        // ------------------ Generic TXT Save/Load ------------------

        private async Task SaveLinesAsync(List<string> lines, string fileName)
        {
            await File.WriteAllLinesAsync(Path.Combine(_folderPath, fileName), lines);
        }

        private async Task<List<string>> LoadLinesAsync(string fileName)
        {
            string path = Path.Combine(_folderPath, fileName);
            if (!File.Exists(path)) return new List<string>();
            return new List<string>(await File.ReadAllLinesAsync(path));
        }

        // ------------------ Entity Specific Save/Load ------------------

        public async Task SaveAirportsAsync(List<Airport> airports)
        {
            var lines = new List<string>();
            foreach (var a in airports)
                lines.Add($"{a.AirportId}|{a.IATA}|{a.Name}|{a.City}|{a.Country}|{a.TimeZone}");
            await SaveLinesAsync(lines, "Airports.txt");
        }

        public async Task<List<Airport>> LoadAirportsAsync()
        {
            var list = new List<Airport>();
            foreach (var line in await LoadLinesAsync("Airports.txt"))
            {
                var p = line.Split('|');
                list.Add(new Airport
                {
                    AirportId = int.Parse(p[0]),
                    IATA = p[1],
                    Name = p[2],
                    City = p[3],
                    Country = p[4],
                    TimeZone = p[5]
                });
            }
            return list;
        }

        public async Task SaveAircraftsAsync(List<Aircraft> aircrafts)
        {
            var lines = new List<string>();
            foreach (var a in aircrafts)
                lines.Add($"{a.AircraftId}|{a.TailNumber}|{a.Model}|{a.Capacity}");
            await SaveLinesAsync(lines, "Aircrafts.txt");
        }

        public async Task<List<Aircraft>> LoadAircraftsAsync()
        {
            var list = new List<Aircraft>();
            foreach (var line in await LoadLinesAsync("Aircrafts.txt"))
            {
                var p = line.Split('|');
                list.Add(new Aircraft
                {
                    AircraftId = int.Parse(p[0]),
                    TailNumber = p[1],
                    Model = p[2],
                    Capacity = int.Parse(p[3])
                });
            }
            return list;
        }

        public async Task SaveCrewMembersAsync(List<CrewMember> crew)
        {
            var lines = new List<string>();
            foreach (var c in crew)
                lines.Add($"{c.CrewId}|{c.FullName}|{c.Role}|{c.LicenseNo}");
            await SaveLinesAsync(lines, "CrewMembers.txt");
        }

        public async Task<List<CrewMember>> LoadCrewMembersAsync()
        {
            var list = new List<CrewMember>();
            foreach (var line in await LoadLinesAsync("CrewMembers.txt"))
            {
                var p = line.Split('|');
                list.Add(new CrewMember
                {
                    CrewId = int.Parse(p[0]),
                    FullName = p[1],
                    Role = p[2],
                    LicenseNo = p.Length > 3 ? p[3] : null
                });
            }
            return list;
        }

        public async Task SaveRoutesAsync(List<Route> routes)
        {
            var lines = new List<string>();
            foreach (var r in routes)
                lines.Add($"{r.RouteId}|{r.DistanceKm}|{r.OriginAirportId}|{r.DestinationAirportId}");
            await SaveLinesAsync(lines, "Routes.txt");
        }

        public async Task<List<Route>> LoadRoutesAsync()
        {
            var list = new List<Route>();
            foreach (var line in await LoadLinesAsync("Routes.txt"))
            {
                var p = line.Split('|');
                list.Add(new Route
                {
                    RouteId = int.Parse(p[0]),
                    DistanceKm = int.Parse(p[1]),
                    OriginAirportId = int.Parse(p[2]),
                    DestinationAirportId = int.Parse(p[3])
                });
            }
            return list;
        }

        public async Task SaveFlightsAsync(List<Flight> flights)
        {
            var lines = new List<string>();
            foreach (var f in flights)
                lines.Add($"{f.FlightId}|{f.FlightNumber}|{f.DepartureUtc:o}|{f.ArrivalUtc:o}|{f.Status}|{f.RouteId}|{f.AircraftId}");
            await SaveLinesAsync(lines, "Flights.txt");
        }

        public async Task<List<Flight>> LoadFlightsAsync()
        {
            var list = new List<Flight>();
            foreach (var line in await LoadLinesAsync("Flights.txt"))
            {
                var p = line.Split('|');
                list.Add(new Flight
                {
                    FlightId = int.Parse(p[0]),
                    FlightNumber = p[1],
                    DepartureUtc = DateTime.Parse(p[2]),
                    ArrivalUtc = DateTime.Parse(p[3]),
                    Status = p[4],
                    RouteId = int.Parse(p[5]),
                    AircraftId = int.Parse(p[6])
                });
            }
            return list;
        }

        public async Task SavePassengersAsync(List<Passenger> passengers)
        {
            var lines = new List<string>();
            foreach (var p in passengers)
                lines.Add($"{p.PassengerId}|{p.FullName}|{p.PassportNo}|{p.Nationality}|{p.DOB:o}");
            await SaveLinesAsync(lines, "Passengers.txt");
        }

        public async Task<List<Passenger>> LoadPassengersAsync()
        {
            var list = new List<Passenger>();
            foreach (var line in await LoadLinesAsync("Passengers.txt"))
            {
                var p = line.Split('|');
                list.Add(new Passenger
                {
                    PassengerId = int.Parse(p[0]),
                    FullName = p[1],
                    PassportNo = p[2],
                    Nationality = p[3],
                    DOB = DateTime.Parse(p[4])
                });
            }
            return list;
        }

        public async Task SaveBookingsAsync(List<Booking> bookings)
        {
            var lines = new List<string>();
            foreach (var b in bookings)
                lines.Add($"{b.BookingId}|{b.BookingRef}|{b.BookingDate:o}|{b.Status}|{b.PassengerId}");
            await SaveLinesAsync(lines, "Bookings.txt");
        }

        public async Task<List<Booking>> LoadBookingsAsync()
        {
            var list = new List<Booking>();
            foreach (var line in await LoadLinesAsync("Bookings.txt"))
            {
                var p = line.Split('|');
                list.Add(new Booking
                {
                    BookingId = int.Parse(p[0]),
                    BookingRef = p[1],
                    BookingDate = DateTime.Parse(p[2]),
                    Status = p[3],
                    PassengerId = int.Parse(p[4])
                });
            }
            return list;
        }

        public async Task SaveTicketsAsync(List<Ticket> tickets)
        {
            var lines = new List<string>();
            foreach (var t in tickets)
                lines.Add($"{t.TicketId}|{t.SeatNumber}|{t.Fare}|{t.CheckedIn}|{t.BookingId}|{t.FlightId}");
            await SaveLinesAsync(lines, "Tickets.txt");
        }

        public async Task<List<Ticket>> LoadTicketsAsync()
        {
            var list = new List<Ticket>();
            foreach (var line in await LoadLinesAsync("Tickets.txt"))
            {
                var p = line.Split('|');
                list.Add(new Ticket
                {
                    TicketId = int.Parse(p[0]),
                    SeatNumber = p[1],
                    Fare = decimal.Parse(p[2]),
                    CheckedIn = bool.Parse(p[3]),
                    BookingId = int.Parse(p[4]),
                    FlightId = int.Parse(p[5])
                });
            }
            return list;
        }

        public async Task SaveFlightCrewsAsync(List<FlightCrew> flightCrews)
        {
            var lines = new List<string>();
            foreach (var fc in flightCrews)
                lines.Add($"{fc.FlightId}|{fc.CrewId}|{fc.RoleOnFlight}");
            await SaveLinesAsync(lines, "FlightCrews.txt");
        }

        public async Task<List<FlightCrew>> LoadFlightCrewsAsync()
        {
            var list = new List<FlightCrew>();
            foreach (var line in await LoadLinesAsync("FlightCrews.txt"))
            {
                var p = line.Split('|');
                list.Add(new FlightCrew
                {
                    FlightId = int.Parse(p[0]),
                    CrewId = int.Parse(p[1]),
                    RoleOnFlight = p[2]
                });
            }
            return list;
        }

        public async Task SaveBaggagesAsync(List<Baggage> baggages)
        {
            var lines = new List<string>();
            foreach (var b in baggages)
                lines.Add($"{b.BaggageId}|{b.TicketId}|{b.WeightKg}|{b.TagNumber}");
            await SaveLinesAsync(lines, "Baggages.txt");
        }

        public async Task<List<Baggage>> LoadBaggagesAsync()
        {
            var list = new List<Baggage>();
            foreach (var line in await LoadLinesAsync("Baggages.txt"))
            {
                var p = line.Split('|');
                list.Add(new Baggage
                {
                    BaggageId = int.Parse(p[0]),
                    TicketId = int.Parse(p[1]),
                    WeightKg = decimal.Parse(p[2]),
                    TagNumber = p[3]
                });
            }
            return list;
        }

        public async Task SaveMaintenancesAsync(List<AircraftMaintenance> maintenances)
        {
            var lines = new List<string>();
            foreach (var m in maintenances)
                lines.Add($"{m.MaintenanceId}|{m.AircraftId}|{m.MaintenanceDate:o}|{m.Type}|{m.Notes}");
            await SaveLinesAsync(lines, "Maintenances.txt");
        }

        public async Task<List<AircraftMaintenance>> LoadMaintenancesAsync()
        {
            var list = new List<AircraftMaintenance>();
            foreach (var line in await LoadLinesAsync("Maintenances.txt"))
            {
                var p = line.Split('|');
                list.Add(new AircraftMaintenance
                {
                    MaintenanceId = int.Parse(p[0]),
                    AircraftId = int.Parse(p[1]),
                    MaintenanceDate = DateTime.Parse(p[2]),
                    Type = p[3],
                    Notes = p.Length > 4 ? p[4] : null
                });
            }
            return list;
        }
    }
}

