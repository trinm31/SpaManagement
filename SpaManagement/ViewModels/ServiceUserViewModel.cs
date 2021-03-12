using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ServiceUserViewModel
    {
        public IEnumerable<SelectListItem> StaffList { get; set; }
        public IEnumerable<SelectListItem> ServiceDetailsList { get; set; }
        public ServiceUsers ServiceUsers { get; set; }
    }
}