using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SpaManagement.ViewModels
{
    public class ServiceUseViewModel
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string Note { get; set; }
        public IEnumerable<SelectListItem> StaffList { get; set; }
        [Required]
        public string StaffId { get; set; }
        public double Paid { get; set; }
    }
}