using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.DTOs.CRUD
{
    public class BaggageUpsertDto
    {
        [Required] public int TicketId { get; set; }
        [Range(0, 999.99)] public decimal WeightKg { get; set; }
        [Required, StringLength(50)] public string TagNumber { get; set; } = string.Empty;
    }
    public class BaggageReadDto : BaggageUpsertDto
    {
        public int BaggageId { get; set; }
    }
}
