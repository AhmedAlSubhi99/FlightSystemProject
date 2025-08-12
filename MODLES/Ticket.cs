using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public string? SeatNumber { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Fare { get; set; }

        [Required]
        public bool CheckedIn { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }

        [ForeignKey(nameof(FlightId))]
        public Flight? Flight { get; set; }

        public ICollection<Baggage>? BaggageItems { get; set; }
    }
}
