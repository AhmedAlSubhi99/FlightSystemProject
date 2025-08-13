using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.MODLES
{
    public class Baggage
    {
        [Key]
        public int BaggageId { get; set; }

        [Required]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        [Precision(6, 2)]
        public decimal WeightKg { get; set; }

        [Required, StringLength(50)]
        public string TagNumber { get; set; } = string.Empty;
    }
}
