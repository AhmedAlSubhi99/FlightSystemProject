using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public interface ICrewRepository : IGenericRepository<CrewMember>
    {
        Task<List<CrewMember>> GetCrewByRoleAsync(string role);
        Task<List<CrewMember>> GetAvailableCrewAsync(DateTime dep);
    }
}
