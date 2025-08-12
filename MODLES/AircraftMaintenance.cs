using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class AircraftMaintenance
    {
        [Key]
        public int MaintenanceId { get; set; }

        [Required]
        public int AircraftId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        public string? Type { get; set; }

        public string? Notes { get; set; }

        [ForeignKey(nameof(AircraftId))]
        public Aircraft? Aircraft { get; set; }
    }
}
