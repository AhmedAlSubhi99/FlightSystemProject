using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Repositories
{
    public class BaggageRepository : GenericRepository<Baggage>, IBaggageRepository
    {
        public BaggageRepository(FlightContext context) : base(context) { }
    }
}