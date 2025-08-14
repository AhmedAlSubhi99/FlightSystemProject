using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightContext _ctx;
        public FlightRepository(FlightContext ctx) => _ctx = ctx;

        public IEnumerable<Flight> GetAll() => _ctx.Flights.ToList();
        public Flight? GetById(int id) => _ctx.Flights.Find(id);
        public void Add(Flight e) { _ctx.Flights.Add(e); _ctx.SaveChanges(); }
        public void Update(Flight e) { _ctx.Flights.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id)
        {
            var e = GetById(id);
            if (e == null) return;
            _ctx.Flights.Remove(e);
            _ctx.SaveChanges();
        }
        public IEnumerable<Flight> GetByDateRange(DateTime from, DateTime to) =>
            _ctx.Flights.Include(f => f.Route!).ThenInclude(r => r.OriginAirport)
                        .Include(f => f.Route!).ThenInclude(r => r.DestinationAirport)
                        .Include(f => f.Aircraft)
                        .Where(f => f.DepartureUtc >= from && f.DepartureUtc <= to)
                        .ToList();

        public IEnumerable<Flight> GetByRoute(int routeId) =>
            _ctx.Flights.Include(f => f.Aircraft).Where(f => f.RouteId == routeId).ToList();

        public Flight? GetWithTicketsAndCrew(int flightId) =>
            _ctx.Flights
                .Include(f => f.Tickets).ThenInclude(t => t.BaggageItems)
                .Include(f => f.FlightCrews).ThenInclude(fc => fc.CrewMember)
                .FirstOrDefault(f => f.FlightId == flightId);
    }
}
