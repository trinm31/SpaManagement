using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Address { get; set; }
        [Required] 
        public string PhoneNumber { get; set; }
        [Required] 
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required] 
        public string ContactVia { get; set; }
        [Required] 
        public string BranchCode { get; set; }
        
    }
}