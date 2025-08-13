using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class TicketUpsertDto
    {
        [Required, StringLength(10)] public string SeatNumber { get; set; } = string.Empty;
        [Range(0, 999999)] public decimal Fare { get; set; }
        public bool CheckedIn { get; set; }
        [Required] public int FlightId { get; set; }
        [Required] public int BookingId { get; set; }
    }
    public class TicketReadDto : TicketUpsertDto
    {
        public int TicketId { get; set; }
    }
}
