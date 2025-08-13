using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class RouteUpsertDto
    {
        [Range(0, int.MaxValue)] public int DistanceKm { get; set; }
        [Required] public int OriginAirportId { get; set; }
        [Required] public int DestinationAirportId { get; set; }
    }
    public class RouteReadDto : RouteUpsertDto
    {
        public int RouteId { get; set; }
        public string? OriginIATA { get; set; }
        public string? DestinationIATA { get; set; }
    }
}
