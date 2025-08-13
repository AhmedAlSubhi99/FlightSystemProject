using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly FlightContext _ctx;
        public TicketRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Ticket> GetAll() => _ctx.Tickets.ToList();
        public Ticket? GetById(int id) => _ctx.Tickets.Find(id);
        public void Add(Ticket e) { _ctx.Tickets.Add(e); _ctx.SaveChanges(); }
        public void Update(Ticket e) { _ctx.Tickets.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Tickets.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<Ticket> GetByBookingRef(string bookingRef) =>
            _ctx.Tickets.Include(t => t.Booking)
                        .Where(t => t.Booking!.BookingRef == bookingRef)
                        .ToList();

        public IEnumerable<Ticket> GetByPassenger(int passengerId) =>
            _ctx.Tickets.Include(t => t.Booking)
                        .Where(t => t.Booking!.PassengerId == passengerId)
                        .ToList();
    }
}
