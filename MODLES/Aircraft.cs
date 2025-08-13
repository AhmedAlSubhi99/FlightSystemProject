using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Aircraft
    {
        [Key]
        public int AircraftId { get; set; }

        [Required, StringLength(50)]
        public string TailNumber { get; set; } = string.Empty; // unique (DbContext)

        [Required, StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Capacity { get; set; }

        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
        public ICollection<AircraftMaintenance> Maintenances { get; set; } = new List<AircraftMaintenance>();
    }
}
