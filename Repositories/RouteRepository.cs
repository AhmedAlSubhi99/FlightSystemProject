using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly FlightContext _ctx;
        public RouteRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Route> GetAll() => _ctx.Routes.ToList();
        public Route? GetById(int id) => _ctx.Routes.Find(id);
        public void Add(Route e) { _ctx.Routes.Add(e); _ctx.SaveChanges(); }
        public void Update(Route e) { _ctx.Routes.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Routes.Remove(e); _ctx.SaveChanges(); } }

        public Route? GetByAirports(int originAirportId, int destinationAirportId) =>
            _ctx.Routes.FirstOrDefault(r => r.OriginAirportId == originAirportId && r.DestinationAirportId == destinationAirportId);

        public IEnumerable<Route> GetWithFlights() => _ctx.Routes.Include(r => r.Flights).ToList();
    }
}
