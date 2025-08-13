using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class AvailableSeatDto
    {
        public int FlightId { get; set; }
        public List<string> Seats { get; set; } = new();
    }
}
