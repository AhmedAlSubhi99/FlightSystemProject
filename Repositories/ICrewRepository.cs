using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace FlightSystemUsingAPI.Repositories
{
    public interface ICrewRepository : IGenericRepository<CrewMember>
    {
        Task<List<CrewMember>> GetCrewByRoleAsync(string role);
        Task<List<CrewMember>> GetAvailableCrewAsync(DateTime dep);
    }
}
