using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class AircraftUpsertDto
    {
        [Required, StringLength(50)] public string TailNumber { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Model { get; set; } = string.Empty;
        [Range(1, 1000)] public int Capacity { get; set; }
    }
    public class AircraftReadDto : AircraftUpsertDto
    {
        public int AircraftId { get; set; }
    }
}
