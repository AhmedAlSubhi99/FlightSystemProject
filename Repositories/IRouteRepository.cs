using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IRouteRepository
    {
        IEnumerable<Route> GetAll();
        Route? GetById(int id);
        void Add(Route entity);
        void Update(Route entity);
        void Delete(int id);

        // Helpers
        Route? GetByAirports(int originAirportId, int destinationAirportId);
        IEnumerable<Route> GetWithFlights();
    }
}
