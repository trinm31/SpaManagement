using System;
using System.ComponentModel.DataAnnotations;
using SpaManagement.Utility.Enum;

namespace SpaManagement.Models
{
    public class Customer : ApplicationUser
    {
        [Required]
        public string IdentityCard { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public Gender Sex { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}