using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<SelectListItem> CustomersList { get; set; }
        public Order Order { get; set; }
    }
}