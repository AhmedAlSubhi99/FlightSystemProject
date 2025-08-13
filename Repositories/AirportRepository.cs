using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly FlightContext _ctx;
        public AirportRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Airport> GetAll() => _ctx.Airports.ToList();
        public Airport? GetById(int id) => _ctx.Airports.Find(id);
        public void Add(Airport e) { _ctx.Airports.Add(e); _ctx.SaveChanges(); }
        public void Update(Airport e) { _ctx.Airports.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Airports.Remove(e); _ctx.SaveChanges(); } }

        public Airport? GetByIata(string iata) => _ctx.Airports.FirstOrDefault(a => a.IATA == iata);
        public Airport? GetWithRoutes(int airportId) =>
            _ctx.Airports.Include(a => a.OriginRoutes).Include(a => a.DestinationRoutes)
                .FirstOrDefault(a => a.AirportId == airportId);
    }
}
