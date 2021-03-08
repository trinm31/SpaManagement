using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Models
{
    public class TypeOfProduct
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string TypeCode { get; set; }
        [Required] 
        public string Name { get; set; }
    }
}