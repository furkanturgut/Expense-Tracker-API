using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string? Description { get; set; }=null;
        public decimal Cost { get; set; }
        public DateTime CreatedDayTime { get; set; }= DateTime.Now;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}