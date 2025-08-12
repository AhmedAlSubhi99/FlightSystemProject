using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.Repositories
{
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        public AirportRepository(FlightContext context) : base(context) { }
    }
}
