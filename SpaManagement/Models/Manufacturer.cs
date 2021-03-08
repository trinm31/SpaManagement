using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Models
{
    public class Manufacturer
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string ManufacturerCode { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}