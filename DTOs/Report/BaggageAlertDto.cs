using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{

    public class BaggageAlertDto
    {
        public int TicketId { get; set; }
        public string BookingRef { get; set; } = string.Empty;
        public decimal TotalWeightKg { get; set; }
    }
}
