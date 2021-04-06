using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class SoldOrderSummaryViewModel
    {
        public int BranchId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<SoldDetailViewModel> ProductList { get; set; }
        public ProductDetail ProductDetails { get; set; }
        public Order Order { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Discount { get; set; }
        public string Note { get; set; }
    }
}