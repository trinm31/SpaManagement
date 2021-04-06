using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpaManagement.Utility.Enum;

namespace SpaManagement.Models
{
    public class Order
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public OrderType OrderType { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double PaidAmount { get; set; }
        [Required]
        public double Debt { get; set; }
        public string Note { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public Customer Customer { get; set; }
    }
}