using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.ExpenseDtos
{
    public class GetExpenseDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }=null;
        public DateTime CreatedDate { get; set; }
        public decimal Cost { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}