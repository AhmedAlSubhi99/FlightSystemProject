using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? PassportNo { get; set; }

        [Required]
        public string? Nationality { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
