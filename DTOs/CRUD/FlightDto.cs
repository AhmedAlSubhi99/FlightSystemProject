using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class FlightUpsertDto
    {
        [Required, StringLength(20)] public string FlightNumber { get; set; } = string.Empty;
        [Required] public DateTime DepartureUtc { get; set; }
        [Required] public DateTime ArrivalUtc { get; set; }
        [Required] public FlightStatus Status { get; set; }
        [Required] public int RouteId { get; set; }
        [Required] public int AircraftId { get; set; }
    }
    public class FlightReadDto : FlightUpsertDto
    {
        public int FlightId { get; set; }
        public string? OriginIATA { get; set; }
        public string? DestinationIATA { get; set; }
        public string? AircraftTail { get; set; }
    }
}
