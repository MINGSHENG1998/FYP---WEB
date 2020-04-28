using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPWEB.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        public int DocID { get; set; }
        public string DocName { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int ConsultationID { get; set; }
    }
}
