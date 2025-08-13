using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly FlightContext _ctx;
        public MaintenanceRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<AircraftMaintenance> GetAll() => _ctx.AircraftMaintenances.ToList();
        public AircraftMaintenance? GetById(int id) => _ctx.AircraftMaintenances.Find(id);
        public void Add(AircraftMaintenance e) { _ctx.AircraftMaintenances.Add(e); _ctx.SaveChanges(); }
        public void Update(AircraftMaintenance e) { _ctx.AircraftMaintenances.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.AircraftMaintenances.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<AircraftMaintenance> GetByAircraft(int aircraftId) =>
            _ctx.AircraftMaintenances.Where(m => m.AircraftId == aircraftId).ToList();

        public IEnumerable<AircraftMaintenance> GetRecent(DateTime since) =>
            _ctx.AircraftMaintenances.Where(m => m.MaintenanceDate >= since).ToList();
    }
}
