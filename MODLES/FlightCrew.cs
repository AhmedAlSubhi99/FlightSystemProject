using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class FlightCrew
    {
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }

        public int CrewId { get; set; }
        public CrewMember? CrewMember { get; set; }

        [Required, StringLength(50)]
        public string RoleOnFlight { get; set; } = string.Empty;
    }
}
