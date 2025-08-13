
using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IPassengerRepository
    {
        IEnumerable<Passenger> GetAll();
        Passenger? GetById(int id);
        void Add(Passenger entity);
        void Update(Passenger entity);
        void Delete(int id);

        // Helpers
        Passenger? GetByPassport(string passportNo);
        Passenger? GetWithBookings(int passengerId);
    }
}
