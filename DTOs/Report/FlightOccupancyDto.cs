using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class FlightOccupancyDto
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public int Sold { get; set; }
        public int Capacity { get; set; }
        public double OccupancyRate => Capacity == 0 ? 0 : (double)Sold / Capacity;
    }
}
