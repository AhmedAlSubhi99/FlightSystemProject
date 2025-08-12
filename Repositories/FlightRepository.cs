using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(FlightContext context) : base(context) { }

        public async Task<List<Flight>> GetFlightsByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(f => f.DepartureUtc >= from && f.DepartureUtc <= to)
                .Include(f => f.Route)
                .Include(f => f.Aircraft)
                .ToListAsync();
        }

        public async Task<List<Flight>> GetFlightsByRouteAsync(int routeId)
        {
            return await _dbSet
                .Where(f => f.RouteId == routeId)
                .Include(f => f.Route)
                .ToListAsync();
        }
    }
}