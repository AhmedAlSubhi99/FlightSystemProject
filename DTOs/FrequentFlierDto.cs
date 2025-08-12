using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs
{
    public class FrequentFlierDto
    {
        public int PassengerId { get; set; }
        public string? FullName { get; set; }
        public int FlightsTaken { get; set; }
        public double TotalDistanceKm { get; set; }
    }
}
