using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.Repositories
{
    public class PassengerRepository : GenericRepository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(FlightContext context) : base(context) { }
    }
}
