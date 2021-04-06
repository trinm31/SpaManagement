using SpaManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaManagement.ReportModel
{
    public class Invoice
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string IdentityCard { get; set; }
        public List<SoldDetailViewModel> productList {get;set;}
    }
}
