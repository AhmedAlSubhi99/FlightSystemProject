using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class AirportUpsertDto
    {
        [Required, StringLength(3)] public string IATA { get; set; } = string.Empty;
        [Required, StringLength(200)] public string Name { get; set; } = string.Empty;
        [Required, StringLength(100)] public string City { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Country { get; set; } = string.Empty;
        [Required, StringLength(100)] public string TimeZone { get; set; } = string.Empty;
    }
    public class AirportReadDto : AirportUpsertDto
    {
        public int AirportId { get; set; }
    }
}
