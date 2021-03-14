using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ProductDetailViewModel
    {
        public IEnumerable<SelectListItem> BranchList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        public ProductDetail ProductDetails { get; set; }
    }
}