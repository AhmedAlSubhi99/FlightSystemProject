using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required, StringLength(10)]
        public string SeatNumber { get; set; } = string.Empty;

        [Precision(10, 2)]
        public decimal Fare { get; set; }

        public bool CheckedIn { get; set; }

        // FKs
        [Required]
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }

        [Required]
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        public ICollection<Baggage> BaggageItems { get; set; } = new List<Baggage>();
    }
}
