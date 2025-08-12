using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FlightSystemUsingAPI.Repositories
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<List<Flight>> GetFlightsByDateRangeAsync(DateTime from, DateTime to);
        Task<List<Flight>> GetFlightsByRouteAsync(int routeId);
    }
}
