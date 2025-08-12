using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(FlightContext context) : base(context) { }

        public async Task<List<Ticket>> GetTicketsByBookingAsync(string bookingRef)
        {
            return await _context.Tickets
                .Include(t => t.Booking)
                .Where(t => t.Booking != null && t.Booking.BookingRef == bookingRef)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByPassengerAsync(int passengerId)
        {
            return await _context.Tickets
                .Include(t => t.Booking)
                .Where(t => t.Booking != null && t.Booking.PassengerId == passengerId)
                .ToListAsync();
        }

    }
}