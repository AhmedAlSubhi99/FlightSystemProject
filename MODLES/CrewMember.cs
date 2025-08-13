using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace FlightSystemUsingAPI.MODLES
{
    public class CrewMember
    {
        [Key]
        public int CrewId { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public CrewRole Role { get; set; }

        [StringLength(100)]
        public string? LicenseNo { get; set; } // nullable

        public ICollection<FlightCrew> FlightCrews { get; set; } = new List<FlightCrew>();
    }
}
