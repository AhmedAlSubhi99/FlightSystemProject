using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class RouteRevenueDto
    {
        public int RouteId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int SeatsSold { get; set; }
        public decimal AvgFare { get; set; }
    }
}
