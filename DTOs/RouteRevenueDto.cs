using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs
{
    public class RouteRevenueDto
    {
        public int RouteId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public decimal Revenue { get; set; }
        public int SeatsSold { get; set; }
        public decimal AvgFare { get; set; }
    }
}
