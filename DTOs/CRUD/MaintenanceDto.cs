using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class MaintenanceUpsertDto
    {
        [Required] public DateTime MaintenanceDate { get; set; }
        [Required, StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(1000)] public string? Notes { get; set; }
        [Required] public int AircraftId { get; set; }
    }
    public class MaintenanceReadDto : MaintenanceUpsertDto
    {
        public int MaintenanceId { get; set; }
    }
}
