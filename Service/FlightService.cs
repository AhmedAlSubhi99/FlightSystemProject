// ======================= FlightService.cs =======================
using FlightSystemUsingAPI.DTOs.Report;
using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Repositories;
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

        //  Implements interface method; uses repos only (no _context)
        public async Task<List<FlightManifestDto>> GetDailyFlightManifestAsync(DateTime date)
        {
            var flights = await _flightRepo.GetFlightsByDateRangeAsync(date.Date, date.Date.AddDays(1));

            return flights.Select(f => new FlightManifestDto
            {
                FlightNumber = f.FlightNumber,
                Origin = f.Route!.OriginAirport!.IATA,
                Destination = f.Route!.DestinationAirport!.IATA,
                DepUtc = f.DepartureUtc,
                ArrUtc = f.ArrivalUtc,
                AircraftTail = f.Aircraft!.TailNumber,
                PassengerCount = (f.Tickets ?? Array.Empty<Ticket>()).Count,
                TotalBaggageKg = (f.Tickets ?? Array.Empty<Ticket>()).SelectMany(t => t.BaggageItems ?? Array.Empty<Baggage>()).Sum(b => b.WeightKg),
                Crew = (f.FlightCrews ?? Array.Empty<FlightCrew>())
                        .Select(fc => new CrewDto
                        {
                            FullName = fc.CrewMember!.FullName,
                            Role = fc.RoleOnFlight
                        })
                        .ToList()
            }).ToList();
        }

        public Task<List<RouteRevenueDto>> GetTopRoutesByRevenueAsync(DateTime from, DateTime to)
            => Task.FromResult(new List<RouteRevenueDto>());

        public Task<List<CrewConflictDto>> GetCrewSchedulingConflictsAsync(DateTime from, DateTime to)
            => Task.FromResult(new List<CrewConflictDto>());

        public Task<List<AvailableSeatDto>> GetAvailableSeatsForFlightAsync(int flightId)
            => Task.FromResult(new List<AvailableSeatDto>());

        public Task<List<FrequentFlierDto>> GetFrequentFliersAsync(int topN)
            => Task.FromResult(new List<FrequentFlierDto>());

        public Task<List<BaggageAlertDto>> GetBaggageOverweightAlertsAsync(decimal threshold)
            => Task.FromResult(new List<BaggageAlertDto>());

        public Task<decimal> GetOnTimePerformanceAsync(DateTime from, DateTime to, int minutesThreshold)
            => Task.FromResult(0m);

        public Task<List<(int FlightId, double OccupancyRate)>> GetSeatOccupancyHeatmapAsync()
            => Task.FromResult(new List<(int, double)>());

        public Task<List<PassengerItineraryDto>> GetPassengersWithConnectionsAsync(int maxHoursBetweenFlights)
            => Task.FromResult(new List<PassengerItineraryDto>());

        public Task<List<(int AircraftId, string TailNumber)>> GetMaintenanceAlertsAsync(double flightHoursThreshold, int daysSinceLastMaintenance)
            => Task.FromResult(new List<(int, string)>());

        public Task<(List<int> Union, List<int> Intersect, List<int> Except)> GetComplexSetExamplesAsync()
            => Task.FromResult((new List<int>(), new List<int>(), new List<int>()));

        public Task<(Dictionary<string, int> FlightsDict, string[] TopRoutes, IEnumerable<string> EnumerableExample)> GetConversionOperatorsDemoAsync()
            => Task.FromResult((new Dictionary<string, int>(), Array.Empty<string>(), Enumerable.Empty<string>()));

        public Task<List<(DateTime Date, decimal RunningRevenue)>> GetRunningRevenueAsync()
            => Task.FromResult(new List<(DateTime, decimal)>());

        public Task<int> ForecastBookingsAsync()
            => Task.FromResult(0);
    }
}
