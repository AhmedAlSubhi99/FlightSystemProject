using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IBaggageRepository
    {
        IEnumerable<Baggage> GetAll();
        Baggage? GetById(int id);
        void Add(Baggage entity);
        void Update(Baggage entity);
        void Delete(int id);

        // Helpers
        IEnumerable<Baggage> GetByTicket(int ticketId);
        IEnumerable<Baggage> GetOverweight(decimal thresholdKg);
    }
}
