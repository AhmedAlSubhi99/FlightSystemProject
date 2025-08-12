using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        public string? FlightNumber { get; set; }

        [Required]
        public DateTime DepartureUtc { get; set; }

        [Required]
        public DateTime ArrivalUtc { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        public int AircraftId { get; set; }

        [ForeignKey(nameof(RouteId))]
        public Route? Route { get; set; }

        [ForeignKey(nameof(AircraftId))]
        public Aircraft? Aircraft { get; set; }

        public ICollection<FlightCrew>? FlightCrews { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
