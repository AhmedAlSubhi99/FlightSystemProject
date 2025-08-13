using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IAirportRepository
    {
        IEnumerable<Airport> GetAll();
        Airport? GetById(int id);
        void Add(Airport entity);
        void Update(Airport entity);
        void Delete(int id);

        // Helpers
        Airport? GetByIata(string iata);
        Airport? GetWithRoutes(int airportId);
    }
}