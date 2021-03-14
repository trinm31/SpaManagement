using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<SelectListItem> TypeOfProducts { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}