using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Airport
    {
        [Key]
        public int AirportId { get; set; }

        [Required, StringLength(3)]
        public string IATA { get; set; } = string.Empty; // unique (DbContext)

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string TimeZone { get; set; } = string.Empty;

        // Navigation
        [InverseProperty(nameof(Route.OriginAirport))]
        public ICollection<Route> OriginRoutes { get; set; } = new List<Route>();

        [InverseProperty(nameof(Route.DestinationAirport))]
        public ICollection<Route> DestinationRoutes { get; set; } = new List<Route>();
    }
}
