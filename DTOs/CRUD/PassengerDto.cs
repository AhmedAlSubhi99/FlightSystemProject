using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class PassengerUpsertDto
    {
        [Required, StringLength(150)] public string FullName { get; set; } = string.Empty;
        [Required, StringLength(50)] public string PassportNo { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Nationality { get; set; } = string.Empty;
        [Required] public DateTime DOB { get; set; }
    }
    public class PassengerReadDto : PassengerUpsertDto
    {
        public int PassengerId { get; set; }
    }
}
