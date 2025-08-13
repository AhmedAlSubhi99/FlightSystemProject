using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly FlightContext _ctx;
        public AircraftRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Aircraft> GetAll() => _ctx.Aircrafts.ToList();
        public Aircraft? GetById(int id) => _ctx.Aircrafts.Find(id);
        public void Add(Aircraft e) { _ctx.Aircrafts.Add(e); _ctx.SaveChanges(); }
        public void Update(Aircraft e) { _ctx.Aircrafts.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Aircrafts.Remove(e); _ctx.SaveChanges(); } }

        public Aircraft? GetByTailNumber(string tail) => _ctx.Aircrafts.FirstOrDefault(a => a.TailNumber == tail);

        public IEnumerable<Aircraft> GetDueForMaintenance(DateTime beforeDate) =>
            _ctx.Aircrafts.Include(a => a.Maintenances)
                .Where(a => a.Maintenances.Any(m => m.MaintenanceDate <= beforeDate))
                .ToList();
    }
}
