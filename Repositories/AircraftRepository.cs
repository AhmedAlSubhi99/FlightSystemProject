using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(FlightContext context) : base(context) { }

        public async Task<List<Aircraft>> GetAircraftDueForMaintenanceAsync(DateTime beforeDate)
        {
            return await _context.Aircrafts
                .Where(a => a.Maintenances.Any(m => m.MaintenanceDate <= beforeDate))
                .Include(a => a.Maintenances)
                .ToListAsync();
        }
    }
}
