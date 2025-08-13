using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FlightContext _ctx;
        public BookingRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Booking> GetAll() => _ctx.Bookings.ToList();
        public Booking? GetById(int id) => _ctx.Bookings.Find(id);
        public void Add(Booking e) { _ctx.Bookings.Add(e); _ctx.SaveChanges(); }
        public void Update(Booking e) { _ctx.Bookings.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Bookings.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<Booking> GetByDateRange(DateTime from, DateTime to) =>
            _ctx.Bookings.Where(b => b.BookingDate >= from && b.BookingDate <= to).ToList();

        public Booking? GetByRef(string bookingRef) =>
            _ctx.Bookings.FirstOrDefault(b => b.BookingRef == bookingRef);

        public Booking? GetWithTickets(string bookingRef) =>
            _ctx.Bookings.Include(b => b.Tickets).FirstOrDefault(b => b.BookingRef == bookingRef);
    }
}
