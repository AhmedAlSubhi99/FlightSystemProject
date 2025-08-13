using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace FlightSystemUsingAPI.MODLES
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Range(0, int.MaxValue)]
        public int DistanceKm { get; set; }

        // Two FKs to Airport
        [ForeignKey(nameof(OriginAirport)), Required]
        public int OriginAirportId { get; set; }

        [ForeignKey(nameof(DestinationAirport)), Required]
        public int DestinationAirportId { get; set; }

        public Airport? OriginAirport { get; set; }
        public Airport? DestinationAirport { get; set; }

        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
    }
}
