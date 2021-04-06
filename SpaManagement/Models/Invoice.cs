using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaManagement.Dtos
{
    public class Invoice
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
