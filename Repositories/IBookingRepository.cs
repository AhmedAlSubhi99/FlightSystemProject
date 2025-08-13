
using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        Booking? GetById(int id);
        void Add(Booking entity);
        void Update(Booking entity);
        void Delete(int id);

        // Helpers
        IEnumerable<Booking> GetByDateRange(DateTime from, DateTime to);
        Booking? GetByRef(string bookingRef);
        Booking? GetWithTickets(string bookingRef);
    }
}
