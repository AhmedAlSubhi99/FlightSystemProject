using FlightSystemUsingAPI.MODLES;

namespace FlightSystemUsingAPI.Repositories
{
    public class MaintenanceRepository : GenericRepository<AircraftMaintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(FlightContext context) : base(context) { }
    }
}