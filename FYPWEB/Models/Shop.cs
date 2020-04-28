using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPWEB.Models
{
    public class Shop
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Info { get; set; }
        public int Cost{ get; set; }
        public string Pic { get; set; }
    }
}
