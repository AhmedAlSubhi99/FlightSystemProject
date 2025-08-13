using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class PassengerItineraryDto
    {
        public int PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;

        // All flight legs ordered (use ItinSegmentDto with Sequence)
        public List<ItinSegmentDto> Segments { get; set; } = new();

        // Convenience summaries
        public int TotalSegments => Segments.Count;
        public int Connections => TotalSegments > 0 ? TotalSegments - 1 : 0;

        public string Origin =>
            TotalSegments > 0 ? Segments[0].Origin : string.Empty;

        public string Destination =>
            TotalSegments > 0 ? Segments[^1].Destination : string.Empty;

        public TimeSpan TotalDuration =>
            TotalSegments > 0
                ? Segments[^1].ArrUtc - Segments[0].DepUtc
                : TimeSpan.Zero;
    }
}

