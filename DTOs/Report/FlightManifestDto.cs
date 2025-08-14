using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class FlightManifestDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime DepUtc { get; set; }
        public DateTime ArrUtc { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string AircraftTail { get; set; } = string.Empty;
        public int PassengerCount { get; set; }
        public decimal TotalBaggageKg { get; set; }
        public List<CrewDto> Crew { get; set; } = new();

        public FlightManifestDto() { }
        public FlightManifestDto(
            string flightNumber, DateTime depUtc, DateTime arrUtc,
            string originIATA, string destIATA, string aircraftTail,
            int passengerCount, decimal totalBaggageKg, List<CrewDto> crew)
        {
            FlightNumber = flightNumber;
            DepUtc = depUtc;
            ArrUtc = arrUtc;
            Origin = originIATA;
            Destination = destIATA;
            AircraftTail = aircraftTail;
            PassengerCount = passengerCount;
            TotalBaggageKg = totalBaggageKg;
            Crew = crew ?? new();
        }
    }
}
