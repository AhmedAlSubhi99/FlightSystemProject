using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.MODLES
{
    public class Baggage
    {
        [Key]
        public int BaggageId { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal WeightKg { get; set; }

        [Required]
        public string? TagNumber { get; set; }

        [ForeignKey(nameof(TicketId))]
        public Ticket? Ticket { get; set; }
    }
}
