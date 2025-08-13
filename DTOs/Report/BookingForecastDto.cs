using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class BookingForecastDto
    {
        public DateTime Day { get; set; }
        public int ForecastBookings { get; set; }
    }
}
