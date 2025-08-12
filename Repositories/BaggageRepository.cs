using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.Repositories
{
    public class BaggageRepository : GenericRepository<Baggage>, IBaggageRepository
    {
        public BaggageRepository(FlightContext context) : base(context) { }
    }
}