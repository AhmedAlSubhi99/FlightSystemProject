using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Repositories
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsByBookingAsync(string bookingRef);
        Task<List<Ticket>> GetTicketsByPassengerAsync(int passengerId);
    }
}
