using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ReturnViewModels
    {
        public int CustomerId { get; set; }
        public int BranchId { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [Required]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        public string Note { get; set; }
    }
}