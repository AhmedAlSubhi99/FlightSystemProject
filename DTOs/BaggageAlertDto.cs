using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs
{
    public class BaggageAlertDto
    {
        public int TicketId { get; set; }
        public string? PassengerName { get; set; }
        public decimal TotalWeight { get; set; }
    }
}
