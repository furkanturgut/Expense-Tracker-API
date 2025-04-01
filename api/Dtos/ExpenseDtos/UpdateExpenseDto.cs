using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ExpenseDtos
{
    public class UpdateExpenseDto
    {
        public string? Description { get; set; }=null;
        [Required]
        public decimal Cost { get; set; }
    }
}