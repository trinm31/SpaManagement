using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class ServiceOrderSummaryViewModel
    {
        public int CustomerId { get; set; }
        public IEnumerable<ServiceDetailsViewmodel> ServiceList { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        [Required]
        public double Price { get; set; }
        public string Note { get; set; }
    }
}