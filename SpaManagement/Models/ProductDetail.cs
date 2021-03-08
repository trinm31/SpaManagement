using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class ProductDetail
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int BranchID { get; set; }
        [ForeignKey("BranchID")] 
        public Branch Branch { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")] 
        public Product Product { get; set; }
    }
}