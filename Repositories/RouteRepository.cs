using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        public RouteRepository(FlightContext context) : base(context) { }
    }
}