using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPWEB.Models
{
    public class TaskDone
    {
        public int ID { get; set; }
        public string User { get; set; }
        public int TaskID { get; set; }
        public string Done { get; set; }
        public string TaskDay { get; set; }
        public string TaskName { get; set; }
        public int TaskPoint { get; set; }
        public string TaskDifficulty { get; set; }
    }
}
