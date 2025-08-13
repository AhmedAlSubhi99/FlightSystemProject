using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{

    public class FrequentFlierDto
    {
        public int PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public int FlightsCount { get; set; }
        public int TotalDistanceKm { get; set; }
    }
}
