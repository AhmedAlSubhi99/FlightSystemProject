using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class CrewMember
    {
        [Key]
        public int CrewId { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Role { get; set; } // Could be Enum

        public string? LicenseNo { get; set; }

        public ICollection<FlightCrew>? FlightCrews { get; set; }
    }
}
