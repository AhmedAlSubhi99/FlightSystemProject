using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class AircraftMaintenance
    {
        [Key]
        public int MaintenanceId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required, StringLength(100)]
        public string Type { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }

        // FK
        [Required]
        public int AircraftId { get; set; }
        public Aircraft? Aircraft { get; set; }
    }
}
