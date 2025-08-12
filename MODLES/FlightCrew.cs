using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class FlightCrew
    {
        [Required]
        public int FlightId { get; set; }

        [Required]
        public int CrewId { get; set; }

        [Required]
        public string? RoleOnFlight { get; set; }

        [ForeignKey(nameof(FlightId))]
        public Flight? Flight { get; set; }

        [ForeignKey(nameof(CrewId))]
        public CrewMember? CrewMember { get; set; }
    }
}
