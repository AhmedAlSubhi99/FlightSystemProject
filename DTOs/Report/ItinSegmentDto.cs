using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class ItinSegmentDto
    {
        public int FlightId { get; set; }              // helpful for joins
        public string FlightNumber { get; set; } = ""; // non-null
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
        public DateTime DepUtc { get; set; }
        public DateTime ArrUtc { get; set; }

        public int Sequence { get; set; }              // 1,2,3... in itinerary order
        public TimeSpan Duration => ArrUtc - DepUtc;   // convenience
    }
}

