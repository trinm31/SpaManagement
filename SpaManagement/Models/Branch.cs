using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
    }
}