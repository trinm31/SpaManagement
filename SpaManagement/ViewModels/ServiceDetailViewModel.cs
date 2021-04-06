using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ServiceDetailViewModel
    {
        public ServiceDetail ServiceDetail { get; set; }
        public ServiceUsers ServiceUsers { get; set; }
        public Slot Slot { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
        [Required]
        public string StaffId { get; set; }
    }
}