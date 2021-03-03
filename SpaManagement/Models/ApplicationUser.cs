using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SpaManagement.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required] 
        public string Name { get; set; }
    }
}