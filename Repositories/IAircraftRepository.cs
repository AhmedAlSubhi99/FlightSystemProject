using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {
        Task<List<Aircraft>> GetAircraftDueForMaintenanceAsync(DateTime beforeDate);
    }
}
