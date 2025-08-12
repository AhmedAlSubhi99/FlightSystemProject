using FlightSystemUsingAPI.DTOs;
using FlightSystemUsingAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Service
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly ICrewRepository _crewRepo;
        private readonly IPassengerRepository _passengerRepo;
        private readonly IBaggageRepository _baggageRepo;
        private readonly IRouteRepository _routeRepo;
        private readonly IAircraftRepository _aircraftRepo;

        public FlightService(
            IFlightRepository flightRepo,
            ITicketRepository ticketRepo,
            IBookingRepository bookingRepo,
            ICrewRepository crewRepo,
            IPassengerRepository passengerRepo,
            IBaggageRepository baggageRepo,
            IRouteRepository routeRepo,
            IAircraftRepository aircraftRepo)
        {
            _flightRepo = flightRepo;
            _ticketRepo = ticketRepo;
            _bookingRepo = bookingRepo;
            _crewRepo = crewRepo;
            _passengerRepo = passengerRepo;
            _baggageRepo = baggageRepo;
            _routeRepo = routeRepo;
            _aircraftRepo = aircraftRepo;
        }

        public async Task<List<FlightManifestDto>> GetDailyFlightManifestAsync(DateTime date)
        {
            var flights = await _flightRepo.GetFlightsByDateRangeAsync(date.Date, date.Date.AddDays(1));

            return flights.Select(f => new FlightManifestDto
            {
                FlightNumber = f.FlightNumber,
                Origin = f.Route.OriginAirport.IATA,
                Destination = f.Route.DestinationAirport.IATA,
                DepUtc = f.DepartureUtc,
                ArrUtc = f.ArrivalUtc,
                AircraftTail = f.Aircraft.TailNumber,
                PassengerCount = f.Tickets.Count,
                TotalBaggageKg = f.Tickets.Sum(t => t.BaggageItems.Sum(b => b.WeightKg)),
                Crew = f.FlightCrews.Select(fc => new CrewDto
                {
                    FullName = fc.CrewMember.FullName,
                    Role = fc.RoleOnFlight
                }).ToList()
            }).ToList();
        }

        public async Task<List<RouteRevenueDto>> GetTopRoutesByRevenueAsync(DateTime from, DateTime to)
        {
            var flights = await _flightRepo.GetFlightsByDateRangeAsync(from, to);

            return flights
                .GroupBy(f => f.RouteId)
                .Select(g => new RouteRevenueDto
                {
                    RouteId = g.Key,
                    Origin = g.First().Route.OriginAirport.IATA,
                    Destination = g.First().Route.DestinationAirport.IATA,
                    Revenue = g.Sum(f => f.Tickets.Sum(t => t.Fare)),
                    SeatsSold = g.Sum(f => f.Tickets.Count),
                    AvgFare = g.Average(f => f.Tickets.Average(t => t.Fare))
                })
                .OrderByDescending(r => r.Revenue)
                .ToList();
        }

        public async Task<List<CrewConflictDto>> GetCrewSchedulingConflictsAsync(DateTime from, DateTime to)
        {
            var flights = await _flightRepo.GetFlightsByDateRangeAsync(from, to);

            var conflicts = new List<CrewConflictDto>();

            foreach (var crew in flights.SelectMany(f => f.FlightCrews.Select(fc => fc.CrewMember)).Distinct())
            {
                var crewFlights = flights
                    .Where(f => f.FlightCrews.Any(fc => fc.CrewId == crew.CrewId))
                    .OrderBy(f => f.DepartureUtc)
                    .ToList();

                for (int i = 0; i < crewFlights.Count - 1; i++)
                {
                    var flightA = crewFlights[i];
                    var flightB = crewFlights[i + 1];

                    if (flightB.DepartureUtc < flightA.ArrivalUtc)
                    {
                        conflicts.Add(new CrewConflictDto
                        {
                            CrewId = crew.CrewId,
                            CrewName = crew.FullName,
                            FlightAId = flightA.FlightId,
                            FlightBId = flightB.FlightId,
                            FlightADep = flightA.DepartureUtc,
                            FlightBDep = flightB.DepartureUtc
                        });
                    }
                }
            }

            return conflicts;
        }

        public async Task<List<AvailableSeatDto>> GetAvailableSeatsForFlightAsync(int flightId)
        {
            var flight = await _flightRepo.GetByIdAsync(flightId);
            if (flight == null) return new List<AvailableSeatDto>();

            var bookedSeats = flight.Tickets.Select(t => t.SeatNumber).ToList();

            var allSeats = Enumerable.Range(1, flight.Aircraft.Capacity)
                                     .Select(i => $"{i}A")
                                     .ToList();

            var availableSeats = allSeats.Except(bookedSeats)
                                         .Select(s => new AvailableSeatDto { SeatNumber = s })
                                         .ToList();

            return availableSeats;
        }

        public async Task<List<FrequentFlierDto>> GetFrequentFliersAsync(int topN)
        {
            var passengers = await _passengerRepo.GetAllAsync();

            return passengers
                .Select(p => new FrequentFlierDto
                {
                    PassengerId = p.PassengerId,
                    FullName = p.FullName,
                    FlightsTaken = p.Bookings.Sum(b => b.Tickets.Count),
                    TotalDistanceKm = p.Bookings
                        .SelectMany(b => b.Tickets.Select(t => t.Flight.Route.DistanceKm))
                        .Sum()
                })
                .OrderByDescending(p => p.FlightsTaken)
                .Take(topN)
                .ToList();
        }

        public async Task<List<BaggageAlertDto>> GetBaggageOverweightAlertsAsync(decimal threshold)
        {
            var tickets = await _ticketRepo.GetAllAsync();

            return tickets
                .Select(t => new
                {
                    Ticket = t,
                    TotalWeight = t.BaggageItems.Sum(b => b.WeightKg)
                })
                .Where(x => x.TotalWeight > threshold)
                .Select(x => new BaggageAlertDto
                {
                    TicketId = x.Ticket.TicketId,
                    PassengerName = x.Ticket.Booking.Passenger.FullName,
                    TotalWeight = x.TotalWeight
                })
                .ToList();
        }
        public async Task<decimal> GetOnTimePerformanceAsync(DateTime from, DateTime to, int minutesThreshold)
        {
            var flights = await _flightRepo.GetFlightsByDateRangeAsync(from, to);
            var onTimeCount = flights.Count(f => Math.Abs((f.ArrivalUtc - f.DepartureUtc).TotalMinutes) <= minutesThreshold);
            return flights.Count == 0 ? 0 : (decimal)onTimeCount / flights.Count * 100;
        }

        public async Task<List<(int FlightId, double OccupancyRate)>> GetSeatOccupancyHeatmapAsync()
        {
            var flights = await _flightRepo.GetAllAsync();
            return flights.Select(f => (f.FlightId, (double)f.Tickets.Count / f.Aircraft.Capacity * 100))
                          .Where(r => r.Item2 > 80)
                          .ToList();
        }

        public async Task<List<PassengerItineraryDto>> GetPassengersWithConnectionsAsync(int maxHoursBetweenFlights)
        {
            var passengers = await _passengerRepo.GetAllAsync();
            var result = new List<PassengerItineraryDto>();

            foreach (var p in passengers)
            {
                var flights = p.Bookings
                    .SelectMany(b => b.Tickets.Select(t => t.Flight))
                    .OrderBy(f => f.DepartureUtc)
                    .ToList();

                var segments = new List<ItinSegmentDto>();

                for (int i = 0; i < flights.Count - 1; i++)
                {
                    var current = flights[i];
                    var next = flights[i + 1];

                    if ((next.DepartureUtc - current.ArrivalUtc).TotalHours <= maxHoursBetweenFlights)
                    {
                        segments.Add(new ItinSegmentDto
                        {
                            FlightNumber = current.FlightNumber,
                            Origin = current.Route.OriginAirport.IATA,
                            Destination = current.Route.DestinationAirport.IATA,
                            DepUtc = current.DepartureUtc,
                            ArrUtc = current.ArrivalUtc
                        });
                        segments.Add(new ItinSegmentDto
                        {
                            FlightNumber = next.FlightNumber,
                            Origin = next.Route.OriginAirport.IATA,
                            Destination = next.Route.DestinationAirport.IATA,
                            DepUtc = next.DepartureUtc,
                            ArrUtc = next.ArrivalUtc
                        });
                    }
                }

                if (segments.Any())
                {
                    result.Add(new PassengerItineraryDto
                    {
                        PassengerId = p.PassengerId,
                        PassengerName = p.FullName,
                        Segments = segments
                    });
                }
            }

            return result;
        }

        public async Task<List<(int AircraftId, string TailNumber)>> GetMaintenanceAlertsAsync(double flightHoursThreshold, int daysSinceLastMaintenance)
        {
            var aircrafts = await _aircraftRepo.GetAllAsync();
            var alerts = new List<(int, string)>();

            foreach (var a in aircrafts)
            {
                var flightHours = a.Flights.Sum(f => f.Route.DistanceKm / 800.0); // avg 800 km/h
                var lastMaintenance = a.Maintenances.OrderByDescending(m => m.MaintenanceDate).FirstOrDefault();

                if (flightHours > flightHoursThreshold ||
                   (lastMaintenance != null && (DateTime.UtcNow - lastMaintenance.MaintenanceDate).TotalDays > daysSinceLastMaintenance))
                {
                    alerts.Add((a.AircraftId, a.TailNumber));
                }
            }

            return alerts;
        }

        public async Task<(List<int> Union, List<int> Intersect, List<int> Except)> GetComplexSetExamplesAsync()
        {
            var vipPassengers = new List<int> { 1, 2, 3, 4 };
            var frequentFliers = (await GetFrequentFliersAsync(5)).Select(ff => ff.PassengerId).ToList();
            var cancelledPassengers = new List<int> { 3, 5 };

            var union = vipPassengers.Union(frequentFliers).ToList();
            var intersect = vipPassengers.Intersect(frequentFliers).ToList();
            var except = vipPassengers.Except(cancelledPassengers).ToList();

            return (union, intersect, except);
        }

        public async Task<(Dictionary<string, int> FlightsDict, string[] TopRoutes, IEnumerable<string> EnumerableExample)> GetConversionOperatorsDemoAsync()
        {
            var flights = await _flightRepo.GetAllAsync();
            var dict = flights.ToDictionary(f => f.FlightNumber, f => f.FlightId);
            var topRoutes = flights.Select(f => f.RouteId.ToString()).Distinct().Take(10).ToArray();
            var enumerable = flights.AsEnumerable().Select(f => f.FlightNumber);

            return (dict, topRoutes, enumerable);
        }

        public async Task<List<(DateTime Date, decimal RunningRevenue)>> GetRunningRevenueAsync()
        {
            var bookings = await _bookingRepo.GetAllAsync();
            var dailyRevenue = bookings
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(b => b.Tickets.Sum(t => t.Fare)) })
                .OrderBy(r => r.Date)
                .ToList();

            decimal cumulative = 0;
            var result = new List<(DateTime, decimal)>();

            foreach (var dr in dailyRevenue)
            {
                cumulative += dr.Revenue;
                result.Add((dr.Date, cumulative));
            }

            return result;
        }

        public async Task<int> ForecastBookingsAsync()
        {
            var bookings = await _bookingRepo.GetAllAsync();
            if (!bookings.Any()) return 0;

            var avgPerDay = bookings
                .GroupBy(b => b.BookingDate.Date)
                .Average(g => g.Count());

            return (int)Math.Round(avgPerDay * 7); // forecast for next 7 days
        }
    }
}
