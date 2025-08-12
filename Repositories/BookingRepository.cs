using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(FlightContext context) : base(context) { }

        public async Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Bookings
                .Where(b => b.BookingDate >= from && b.BookingDate <= to)
                .ToListAsync();
        }
    }
}