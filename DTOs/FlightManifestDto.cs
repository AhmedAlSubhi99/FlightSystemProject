using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs
{
    public class FlightManifestDto
    {
        public string? FlightNumber { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepUtc { get; set; }
        public DateTime ArrUtc { get; set; }
        public string? AircraftTail { get; set; }
        public int PassengerCount { get; set; }
        public decimal TotalBaggageKg { get; set; }
        public List<CrewDto>? Crew { get; set; }
    }
}
