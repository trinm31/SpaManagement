using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ImportViewModel
    {
        public IEnumerable<SelectListItem> BranchList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        public ProductDetail ProductDetails { get; set; }
        public Order Order { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        [Required]
        public double Price { get; set; }
        public string Note { get; set; }
    }
}