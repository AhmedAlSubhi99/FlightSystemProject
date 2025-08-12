using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public class MaintenanceRepository : GenericRepository<AircraftMaintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(FlightContext context) : base(context) { }
    }
}