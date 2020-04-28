using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPWEB.Models
{
    public class DailyTask
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string Day { get; set; }
        public string Difficulty { get; set; }        
        public int Point { get; set; }
    }
}
