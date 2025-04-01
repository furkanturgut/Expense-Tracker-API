using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helper
{
    public class QueryObject
    {
        public bool PastWeek { get; set; }= false;
        public bool LastMonth { get; set; }=false;
        public bool Last3Months { get; set; }=false;
        public DateTime? StartDate { get; set; }=null;
    }
}