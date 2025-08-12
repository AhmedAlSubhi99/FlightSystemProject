using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs
{
    public class PassengerItineraryDto
    {
        public int PassengerId { get; set; }
        public string? PassengerName { get; set; }
        public List<ItinSegmentDto>? Segments { get; set; }
    }
}
