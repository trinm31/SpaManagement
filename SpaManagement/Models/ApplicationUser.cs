using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SpaManagement.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required] 
        public string Name { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}