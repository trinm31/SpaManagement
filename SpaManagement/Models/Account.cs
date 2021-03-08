using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class Account
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public DateTime TransactDate { get; set; }
        [Required] 
        public double Debt { get; set; }
        [Required] 
        public double Credit { get; set; }
        [Required] 
        public double Balance { get; set; }
        public string Description { get; set; }
        [Required] 
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public Customer Customer { get; set; }
    }
}