using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> GetAll();
        Flight? GetById(int id);
        void Add(Flight entity);
        void Update(Flight entity);
        void Delete(int id);

        // Helpers
        IEnumerable<Flight> GetByDateRange(DateTime from, DateTime to);
        IEnumerable<Flight> GetByRoute(int routeId);
        Flight? GetWithTicketsAndCrew(int flightId);
    }
}
