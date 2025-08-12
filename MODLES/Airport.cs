using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Airport
    {
        [Key]
        public int AirportId { get; set; }

        [Required, StringLength(3)]
        public string? IATA { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? TimeZone { get; set; }

        public ICollection<Route>? OriginRoutes { get; set; }
        public ICollection<Route>? DestinationRoutes { get; set; }
    }
}
