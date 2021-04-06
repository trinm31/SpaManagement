using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaManagement.ReportModel
{
    public class ImportInvoice
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double ImportPrice { get; set; }
        public double Price { get; set; }
        public double Debt { get; set; }
    }
}
