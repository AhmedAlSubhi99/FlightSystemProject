using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IAircraftRepository
    {
        IEnumerable<Aircraft> GetAll();
        Aircraft? GetById(int id);
        void Add(Aircraft entity);
        void Update(Aircraft entity);
        void Delete(int id);

        // Helpers
        Aircraft? GetByTailNumber(string tail);
        IEnumerable<Aircraft> GetDueForMaintenance(DateTime beforeDate);
    }
}
