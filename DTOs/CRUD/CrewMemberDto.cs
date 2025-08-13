using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class CrewMemberUpsertDto
    {
        [Required, StringLength(150)] public string FullName { get; set; } = string.Empty;
        [Required] public CrewRole Role { get; set; }
        [StringLength(100)] public string? LicenseNo { get; set; }
    }
    public class CrewMemberReadDto : CrewMemberUpsertDto
    {
        public int CrewId { get; set; }
    }
}
