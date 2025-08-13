using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required, StringLength(20)]
        public string FlightNumber { get; set; } = string.Empty; // + DepartureUtc.Date unique (DbContext)

        [Required]
        public DateTime DepartureUtc { get; set; }

        [Required]
        public DateTime ArrivalUtc { get; set; }

        [Required]
        public FlightStatus Status { get; set; }

        // FKs
        [Required]
        public int RouteId { get; set; }
        public Route? Route { get; set; }

        [Required]
        public int AircraftId { get; set; }
        public Aircraft? Aircraft { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<FlightCrew> FlightCrews { get; set; } = new List<FlightCrew>();
    }
}
