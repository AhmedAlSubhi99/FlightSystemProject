using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly FlightContext _ctx;
        public PassengerRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Passenger> GetAll() => _ctx.Passengers.ToList();
        public Passenger? GetById(int id) => _ctx.Passengers.Find(id);
        public void Add(Passenger e) { _ctx.Passengers.Add(e); _ctx.SaveChanges(); }
        public void Update(Passenger e) { _ctx.Passengers.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Passengers.Remove(e); _ctx.SaveChanges(); } }

        public Passenger? GetByPassport(string passportNo) =>
            _ctx.Passengers.FirstOrDefault(p => p.PassportNo == passportNo);

        public Passenger? GetWithBookings(int passengerId) =>
            _ctx.Passengers.Include(p => p.Bookings)
                           .ThenInclude(b => b.Tickets)
                           .FirstOrDefault(p => p.PassengerId == passengerId);
    }
}
