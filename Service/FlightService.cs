using FlightSystemUsingAPI.DTOs;
using FlightSystemUsingAPI.DTOs.Report;
using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Service
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepo;
        private readonly IRouteRepository _routeRepo;
        private readonly IAirportRepository _airportRepo;
        private readonly IAircraftRepository _aircraftRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly ICrewRepository _crewRepo;
        private readonly IPassengerRepository _passengerRepo;
        private readonly IBaggageRepository _baggageRepo;
        private readonly IMaintenanceRepository _maintenanceRepo;

        public FlightService(
            IFlightRepository flightRepo,
            IRouteRepository routeRepo,
            IAirportRepository airportRepo,
            IAircraftRepository aircraftRepo,
            ITicketRepository ticketRepo,
            IBookingRepository bookingRepo,
            ICrewRepository crewRepo,
            IPassengerRepository passengerRepo,
            IBaggageRepository baggageRepo,
            IMaintenanceRepository maintenanceRepo)
        {
            _flightRepo = flightRepo;
            _routeRepo = routeRepo;
            _airportRepo = airportRepo;
            _aircraftRepo = aircraftRepo;
            _ticketRepo = ticketRepo;
            _bookingRepo = bookingRepo;
            _crewRepo = crewRepo;
            _passengerRepo = passengerRepo;
            _baggageRepo = baggageRepo;
            _maintenanceRepo = maintenanceRepo;
        }



        // --------------------- 1) Daily Manifest ---------------------
        // In FlightService
        // FlightService.cs
        public List<FlightManifestDto> GetDailyFlightManifest(DateTime dateUtc)
        {
            var start = dateUtc.Date;
            var end = start.AddDays(1);

            var flights = _flightRepo.GetByDateRange(start, end).ToList();

            var routes = _routeRepo.GetAll().ToDictionary(r => r.RouteId);
            var airports = _airportRepo.GetAll().ToDictionary(a => a.AirportId);
            var aircrafts = _aircraftRepo.GetAll().ToDictionary(a => a.AircraftId);

            var ticketsByFlight = _ticketRepo.GetAll()
                .GroupBy(t => t.FlightId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var baggageByTicket = _baggageRepo.GetAll()
                .GroupBy(b => b.TicketId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<FlightManifestDto>();

            foreach (var f in flights)
            {
                var route = routes[f.RouteId];
                var originIata = airports[route.OriginAirportId].IATA;
                var destIata = airports[route.DestinationAirportId].IATA;

                var tail = aircrafts.TryGetValue(f.AircraftId, out var ac) ? ac.TailNumber : string.Empty;

                var tks = ticketsByFlight.TryGetValue(f.FlightId, out var list) ? list : new List<Ticket>();
                var paxCount = tks.Count;

                decimal totalBagsKg = 0m;
                foreach (var t in tks)
                    if (baggageByTicket.TryGetValue(t.TicketId, out var bags))
                        totalBagsKg += bags.Sum(b => b.WeightKg);

                // crew
                var crew = new List<CrewDto>();
                var withCrew = _flightRepo.GetWithTicketsAndCrew(f.FlightId);
                if (withCrew?.FlightCrews != null)
                {
                    foreach (var fc in withCrew.FlightCrews)
                    {
                        var cm = _crewRepo.GetById(fc.CrewId);
                        crew.Add(new CrewDto(cm?.FullName ?? "", cm?.Role.ToString() ?? ""));
                    }
                }

                result.Add(new FlightManifestDto(
                    f.FlightNumber,
                    f.DepartureUtc,
                    f.ArrivalUtc,
                    originIata,
                    destIata,
                    tail,
                    paxCount,
                    totalBagsKg,
                    crew
                ));
            }

            return result.OrderBy(m => m.DepUtc).ToList();
        }



        // --------------------- 2) Top Routes by Revenue ---------------------
        public List<RouteRevenueDto> GetTopRoutesByRevenue(DateTime fromUtc, DateTime toUtc, int take = 10)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();
            var flightIds = flights.Select(f => f.FlightId).ToHashSet();
            var tickets = _ticketRepo.GetAll().Where(t => flightIds.Contains(t.FlightId)).ToList();

            var q = from f in flights
                    join t in tickets on f.FlightId equals t.FlightId into ft
                    group ft by f.RouteId into g
                    let route = _routeRepo.GetById(g.Key)!
                    let origin = _airportRepo.GetById(route.OriginAirportId)!.IATA
                    let dest = _airportRepo.GetById(route.DestinationAirportId)!.IATA
                    select new RouteRevenueDto
                    {
                        RouteId = g.Key,
                        Origin = origin,
                        Destination = dest,
                        Revenue = g.SelectMany(x => x).Sum(x => x.Fare),
                        SeatsSold = g.SelectMany(x => x).Count(),
                        AvgFare = g.SelectMany(x => x).Any() ? g.SelectMany(x => x).Average(x => x.Fare) : 0m
                    };

            var res = q.OrderByDescending(x => x.Revenue).ToList();
            return take > 0 ? res.Take(take).ToList() : res;
        }

        // --------------------- 3) Seat Occupancy ---------------------
        public List<FlightOccupancyDto> GetSeatOccupancy(DateTime fromUtc, DateTime toUtc, double minRate = 0.8, int topN = 0)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();
            var soldByFlight = _ticketRepo.GetAll()
                                          .GroupBy(t => t.FlightId)
                                          .ToDictionary(g => g.Key, g => g.Count());

            var list = flights.Select(f => new FlightOccupancyDto
            {
                FlightId = f.FlightId,
                FlightNumber = f.FlightNumber,
                Sold = soldByFlight.TryGetValue(f.FlightId, out var c) ? c : 0,
                Capacity = f.Aircraft?.Capacity ?? 0
            })
            .Where(x => x.Capacity > 0)
            .ToList();

            var filtered = list.Where(x => x.OccupancyRate >= minRate)
                               .OrderByDescending(x => x.OccupancyRate)
                               .ToList();

            return topN > 0 ? filtered.Take(topN).ToList() : filtered;
        }

        // --------------------- 4) Available Seats for a Flight ---------------------
        public AvailableSeatDto GetAvailableSeatsForFlight(int flightId)
        {
            var f = _flightRepo.GetById(flightId) ?? throw new ArgumentException("Flight not found");
            var capacity = f.Aircraft?.Capacity ?? 0;
            var seatMap = BuildSeatMap(capacity);

            var sold = _ticketRepo.GetAll()
                                  .Where(t => t.FlightId == flightId)
                                  .Select(t => t.SeatNumber)
                                  .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var available = seatMap.Where(s => !sold.Contains(s)).ToList();
            return new AvailableSeatDto { FlightId = flightId, Seats = available };
        }

        // --------------------- 5) Crew Scheduling Conflicts ---------------------
        public List<CrewConflictDto> GetCrewSchedulingConflicts(DateTime fromUtc, DateTime toUtc)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();

            // overlapping flight pairs
            var pairs = (from a in flights
                         from b in flights
                         where a.FlightId < b.FlightId &&
                               a.DepartureUtc < b.ArrivalUtc &&
                               b.DepartureUtc < a.ArrivalUtc
                         select (a, b)).ToList();

            var conflicts = new List<CrewConflictDto>();
            foreach (var (a, b) in pairs)
            {
                var aCrewIds = _flightRepo.GetWithTicketsAndCrew(a.FlightId)!.FlightCrews.Select(x => x.CrewId).ToHashSet();
                var bCrewIds = _flightRepo.GetWithTicketsAndCrew(b.FlightId)!.FlightCrews.Select(x => x.CrewId).ToHashSet();

                foreach (var cid in aCrewIds.Intersect(bCrewIds))
                {
                    var cm = _crewRepo.GetById(cid)!;
                    conflicts.Add(new CrewConflictDto
                    {
                        CrewId = cid,
                        CrewName = cm.FullName,
                        FlightAId = a.FlightId,
                        FlightBId = b.FlightId,
                        FlightADep = a.DepartureUtc,
                        FlightBDep = b.DepartureUtc
                    });
                }
            }

            return conflicts.OrderBy(x => x.CrewId).ThenBy(x => x.FlightADep).ToList();
        }

        // --------------------- 6) Frequent Fliers ---------------------
        public List<FrequentFlierDto> GetFrequentFliers(int topN = 20)
        {
            var tickets = _ticketRepo.GetAll().ToList();
            var bookings = _bookingRepo.GetAll().ToList();
            var flights = _flightRepo.GetAll().ToList();
            var routes = _routeRepo.GetAll().ToList();

            var routeById = routes.ToDictionary(r => r.RouteId, r => r);
            var flightById = flights.ToDictionary(f => f.FlightId, f => f);
            var paxName = _passengerRepo.GetAll().ToDictionary(p => p.PassengerId, p => p.FullName);

            var q = from t in tickets
                    join b in bookings on t.BookingId equals b.BookingId
                    group new { t, b } by b.PassengerId into g
                    let flightsCount = g.Count()
                    let totalDist = g.Sum(x => routeById[flightById[x.t.FlightId].RouteId].DistanceKm)
                    orderby flightsCount descending, totalDist descending
                    select new FrequentFlierDto
                    {
                        PassengerId = g.Key,
                        PassengerName = paxName.TryGetValue(g.Key, out var n) ? n : string.Empty,
                        FlightsCount = flightsCount,
                        TotalDistanceKm = totalDist
                    };

            return (topN > 0 ? q.Take(topN) : q).ToList();
        }

        // --------------------- 7) Baggage Overweight Alerts ---------------------
        public List<BaggageAlertDto> GetBaggageOverweightAlerts(decimal perPassengerThresholdKg = 30m)
        {
            var baggage = _baggageRepo.GetAll().ToList();
            var tickets = _ticketRepo.GetAll().ToList();
            var bookingRefById = _bookingRepo.GetAll().ToDictionary(b => b.BookingId, b => b.BookingRef);

            var q = from b in baggage
                    group b by b.TicketId into g
                    let total = g.Sum(x => x.WeightKg)
                    where total > perPassengerThresholdKg
                    let t = tickets.First(x => x.TicketId == g.Key)
                    select new BaggageAlertDto
                    {
                        TicketId = g.Key,
                        BookingRef = bookingRefById[t.BookingId],
                        TotalWeightKg = total
                    };

            return q.OrderByDescending(x => x.TotalWeightKg).ToList();
        }

        // --------------------- 8) Maintenance Alerts ---------------------
        public List<MaintenanceAlertDto> GetMaintenanceAlerts(DateTime staleBeforeDate, int minFlights = 20)
        {
            var aircraft = _aircraftRepo.GetAll().ToList();
            var flights = _flightRepo.GetAll().ToList();
            var maint = _maintenanceRepo.GetAll().ToList();

            var flightsPerAircraft = flights.GroupBy(f => f.AircraftId)
                                            .ToDictionary(g => g.Key, g => g.Count());

            var lastMaint = maint.GroupBy(m => m.AircraftId)
                                 .ToDictionary(g => g.Key, g => g.Max(x => x.MaintenanceDate));

            var q = from a in aircraft
                    let count = flightsPerAircraft.TryGetValue(a.AircraftId, out var c) ? c : 0
                    let last = lastMaint.TryGetValue(a.AircraftId, out var d) ? d : (DateTime?)null
                    where (last == null || last <= staleBeforeDate) || count >= minFlights
                    select new MaintenanceAlertDto
                    {
                        AircraftId = a.AircraftId,
                        TailNumber = a.TailNumber,
                        LastMaintenance = last,
                        FlightsCount = count
                    };

            return q.OrderByDescending(x => x.FlightsCount).ToList();
        }

        // --------------------- 9) Running Revenue Per Day ---------------------
        public List<DailyRevenuePoint> GetRunningRevenue(DateTime fromUtc, DateTime toUtc)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();
            var flightIds = flights.Select(f => f.FlightId).ToHashSet();
            var tickets = _ticketRepo.GetAll().Where(t => flightIds.Contains(t.FlightId))
                                                .Select(t => new { t.FlightId, t.Fare })
                                                .ToList();

            var daily = from f in flights
                        join t in tickets on f.FlightId equals t.FlightId into ft
                        let day = f.DepartureUtc.Date
                        let rev = ft.Sum(x => x.Fare)
                        group rev by day into g
                        orderby g.Key
                        select new { Day = g.Key, Revenue = g.Sum() };

            decimal running = 0;
            return daily.Select(p => { running += p.Revenue; return new DailyRevenuePoint { Day = p.Day, Revenue = p.Revenue, RunningRevenue = running }; }).ToList();
        }

        // --------------------- 10) Simple Booking Forecast (7 days) ---------------------
        public List<BookingForecastDto> ForecastNextWeekBookings()
        {
            var bookings = _bookingRepo.GetAll().ToList();
            var byDay = bookings.GroupBy(b => b.BookingDate.Date)
                                .ToDictionary(g => g.Key, g => g.Count());

            var last14 = Enumerable.Range(1, 14)
                                   .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                                   .Select(d => byDay.TryGetValue(d, out var c) ? c : 0)
                                   .ToList();

            var avg = last14.Any() ? (int)Math.Round(last14.Average()) : 0;

            return Enumerable.Range(1, 7)
                             .Select(i => new BookingForecastDto
                             {
                                 Day = DateTime.UtcNow.Date.AddDays(i),
                                 ForecastBookings = avg
                             })
                             .ToList();
        }

        // --------------------- 11) On-Time Performance ---------------------
        public List<OnTimePerformanceDto> GetOnTimePerformance(DateTime fromUtc, DateTime toUtc)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc);

            var daily = from f in flights
                        let day = f.DepartureUtc.Date
                        group f by day into g
                        orderby g.Key
                        select new OnTimePerformanceDto
                        {
                            Day = g.Key,
                            Scheduled = g.Count(x => x.Status != FlightStatus.Canceled),
                            Landed = g.Count(x => x.Status == FlightStatus.Landed),
                            Delayed = g.Count(x => x.Status == FlightStatus.Delayed)
                        };

            return daily.ToList();
        }

        // --------------------- 12) Passengers with Connections ---------------------
        public List<PassengerItineraryDto> GetPassengersWithConnections(DateTime fromUtc, DateTime toUtc)
        {
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();
            var routeById = _routeRepo.GetAll().ToDictionary(r => r.RouteId, r => r);
            var airportIata = _airportRepo.GetAll().ToDictionary(a => a.AirportId, a => a.IATA);
            var bookings = _bookingRepo.GetAll().ToList();
            var tickets = _ticketRepo.GetAll().ToList();
            var paxNames = _passengerRepo.GetAll().ToDictionary(p => p.PassengerId, p => p.FullName);

            var legs = from t in tickets
                       join b in bookings on t.BookingId equals b.BookingId
                       join f in flights on t.FlightId equals f.FlightId
                       let r = routeById[f.RouteId]
                       select new
                       {
                           b.PassengerId,
                           Flight = f,
                           Origin = airportIata[r.OriginAirportId],
                           Destination = airportIata[r.DestinationAirportId]
                       };

            var itineraries = new List<PassengerItineraryDto>();

            foreach (var g in legs.GroupBy(x => x.PassengerId))
            {
                var ordered = g.OrderBy(x => x.Flight.DepartureUtc).ToList();
                if (ordered.Count < 2) continue; // connection requires ≥2 segments

                var segs = ordered.Select((x, i) => new ItinSegmentDto
                {
                    FlightId = x.Flight.FlightId,
                    FlightNumber = x.Flight.FlightNumber,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    DepUtc = x.Flight.DepartureUtc,
                    ArrUtc = x.Flight.ArrivalUtc,
                    Sequence = i + 1
                }).ToList();

                itineraries.Add(new PassengerItineraryDto
                {
                    PassengerId = g.Key,
                    PassengerName = paxNames.TryGetValue(g.Key, out var n) ? n : string.Empty,
                    Segments = segs
                });
            }

            return itineraries.OrderByDescending(i => i.TotalSegments)
                              .ThenBy(i => i.PassengerName)
                              .ToList();
        }

        // --------------------- 13) Single Passenger Itinerary ---------------------
        public PassengerItineraryDto? GetPassengerItinerary(int passengerId, DateTime fromUtc, DateTime toUtc)
        {
            var all = GetPassengersWithConnections(fromUtc, toUtc);
            var match = all.FirstOrDefault(p => p.PassengerId == passengerId);
            if (match != null) return match;

            // return 0/1-segment itinerary if no connections
            var flights = _flightRepo.GetByDateRange(fromUtc, toUtc).ToList();
            var routeById = _routeRepo.GetAll().ToDictionary(r => r.RouteId, r => r);
            var airportIata = _airportRepo.GetAll().ToDictionary(a => a.AirportId, a => a.IATA);
            var bookings = _bookingRepo.GetAll().Where(b => b.PassengerId == passengerId).ToList();
            var tickets = _ticketRepo.GetAll().Where(t => bookings.Any(b => b.BookingId == t.BookingId)).ToList();
            var name = _passengerRepo.GetById(passengerId)?.FullName ?? string.Empty;

            var segs = (from t in tickets
                        join f in flights on t.FlightId equals f.FlightId
                        let r = routeById[f.RouteId]
                        orderby f.DepartureUtc
                        select new ItinSegmentDto
                        {
                            FlightId = f.FlightId,
                            FlightNumber = f.FlightNumber,
                            Origin = airportIata[r.OriginAirportId],
                            Destination = airportIata[r.DestinationAirportId],
                            DepUtc = f.DepartureUtc,
                            ArrUtc = f.ArrivalUtc,
                        })
                        .Select((x, i) => { x.Sequence = i + 1; return x; })
                        .ToList();

            return new PassengerItineraryDto
            {
                PassengerId = passengerId,
                PassengerName = name,
                Segments = segs
            };
        }

        // --------------------- helper: seat map ---------------------
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
