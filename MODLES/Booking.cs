using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string? BookingRef { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [ForeignKey(nameof(PassengerId))]
        public Passenger? Passenger { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
