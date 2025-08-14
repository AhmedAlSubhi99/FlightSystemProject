using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class OnTimePerformanceDto
    {
        public System.DateTime Day { get; set; }
        public int Scheduled { get; set; }   // all flights that day (non-canceled)
        public int Landed { get; set; }      // completed on time (proxy)
        public int Delayed { get; set; }     // explicitly flagged as delayed
        public double OnTimeRate => Scheduled == 0 ? 0 : (double)Landed / Scheduled;
    }
}
