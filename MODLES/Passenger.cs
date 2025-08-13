using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string PassportNo { get; set; } = string.Empty; // unique (DbContext)

        [Required, StringLength(100)]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        public DateTime DOB { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

}
