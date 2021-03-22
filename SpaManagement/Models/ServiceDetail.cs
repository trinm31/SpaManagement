using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class ServiceDetail
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int Slot { get; set; }
        [Required]
        public double Debt { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Paid { get; set; }
        [Required]
        public double Discount { get; set; }
        [Required] 
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public Customer Customer { get; set; }
        [Required] 
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")] 
        public CategoryService CategoryService { get; set; }
    }
}