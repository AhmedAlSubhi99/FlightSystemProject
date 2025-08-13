using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IMaintenanceRepository
    {
        IEnumerable<AircraftMaintenance> GetAll();
        AircraftMaintenance? GetById(int id);
        void Add(AircraftMaintenance entity);
        void Update(AircraftMaintenance entity);
        void Delete(int id);

        // Helpers
        IEnumerable<AircraftMaintenance> GetByAircraft(int aircraftId);
        IEnumerable<AircraftMaintenance> GetRecent(DateTime since);
    }
}
