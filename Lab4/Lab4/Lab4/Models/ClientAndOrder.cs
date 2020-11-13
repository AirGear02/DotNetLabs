using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class ClientAndOrder
    {
        public DateTime DateStart {get; set;}
        public DateTime DateFinish { get; set; }

        public string Option { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
