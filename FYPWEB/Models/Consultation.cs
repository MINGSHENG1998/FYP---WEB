using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPWEB.Models
{
    public class Consultation
    {
        public int ID { get; set; }
        public int DocID { get; set; }
        public string DocName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}
