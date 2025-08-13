using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class MaintenanceAlertDto
    {
        public int AircraftId { get; set; }
        public string TailNumber { get; set; } = string.Empty;
        public DateTime? LastMaintenance { get; set; }
        public int FlightsCount { get; set; }
    }
}
