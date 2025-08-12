using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.Repositories
{
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        public RouteRepository(FlightContext context) : base(context) { }
    }
}