using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required, StringLength(50)]
        public string BookingRef { get; set; } = string.Empty; // unique (DbContext)

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        // FK
        [Required]
        public int PassengerId { get; set; }
        public Passenger? Passenger { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
