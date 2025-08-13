using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.Report
{
    public class CrewDto
    {
        public int CrewId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string RoleOnFlight { get; set; } = string.Empty;
    }
}
