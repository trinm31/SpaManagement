using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ServiceDetailViewModel
    {
        public IEnumerable<SelectListItem> CategoryServiceList { get; set; }
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public ServiceDetail ServiceDetail { get; set; }
    }
}