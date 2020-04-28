using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPWEB.Models
{
    public class Points
    {
        public int ID { get; set; }
        public string User { get; set; }
        public int TotalPoint { get; set; }
    }
}
