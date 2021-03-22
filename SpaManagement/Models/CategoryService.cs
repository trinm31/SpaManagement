using System.ComponentModel.DataAnnotations;

namespace SpaManagement.Models
{
    public class CategoryService
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string ServiceCode { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public double Price { get; set; }
        [Required] 
        public double Discount { get; set; }
        public string Note { get; set; }
    }
}