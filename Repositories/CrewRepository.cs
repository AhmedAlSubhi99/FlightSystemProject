using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public class CrewRepository : GenericRepository<CrewMember>, ICrewRepository
    {
        public CrewRepository(FlightContext context) : base(context) { }

        public async Task<List<CrewMember>> GetCrewByRoleAsync(string role)
        {
            return await _context.CrewMembers
                .Where(c => c.Role == role)
                .ToListAsync();
        }

        public async Task<List<CrewMember>> GetAvailableCrewAsync(DateTime dep)
        {
            return await _context.CrewMembers
                .Where(c => !c.FlightCrews.Any(fc =>
                    fc.Flight.DepartureUtc <= dep &&
                    fc.Flight.ArrivalUtc >= dep))
                .ToListAsync();
        }
    }
}
