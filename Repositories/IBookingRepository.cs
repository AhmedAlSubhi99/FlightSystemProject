using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FlightSystemUsingAPI.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime from, DateTime to);
    }
}
