using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class AccountViewModel
    {
        public IEnumerable<SelectListItem> CustomersList { get; set; }
        public Account Account { get; set; }
    }
}