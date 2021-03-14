using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<SelectListItem> Products { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}