using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        public int DistanceKm { get; set; }

        [Required]
        public int OriginAirportId { get; set; }

        [Required]
        public int DestinationAirportId { get; set; }

        [ForeignKey(nameof(OriginAirportId))]
        public Airport? OriginAirport { get; set; }

        [ForeignKey(nameof(DestinationAirportId))]
        public Airport? DestinationAirport { get; set; }

        public ICollection<Flight>? Flights { get; set; }
    }
}
