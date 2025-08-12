using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {
        Task<List<Aircraft>> GetAircraftDueForMaintenanceAsync(DateTime beforeDate);
    }
}
