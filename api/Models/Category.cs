using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Expense>? Expenses { get; set; }
    }
}