using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class BookingUpsertDto
    {
        [Required, StringLength(50)] public string BookingRef { get; set; } = string.Empty;
        [Required] public DateTime BookingDate { get; set; }
        [Required] public BookingStatus Status { get; set; }
        [Required] public int PassengerId { get; set; }
    }
    public class BookingReadDto : BookingUpsertDto
    {
        public int BookingId { get; set; }
    }
}
