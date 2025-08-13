
using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAll();
        Ticket? GetById(int id);
        void Add(Ticket entity);
        void Update(Ticket entity);
        void Delete(int id);

        // Helpers
        IEnumerable<Ticket> GetByBookingRef(string bookingRef);
        IEnumerable<Ticket> GetByPassenger(int passengerId);
    }
}
