using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Aircraft
    {
        [Key]
        public int AircraftId { get; set; }

        [Required]
        public string? TailNumber { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public int Capacity { get; set; }

        public ICollection<Flight>? Flights { get; set; }
        public ICollection<AircraftMaintenance>? Maintenances { get; set; }
    }
}
