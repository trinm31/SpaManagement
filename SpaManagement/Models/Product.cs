using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class Product
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Description { get; set; }
        [Required]
        public double ImportPrice { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string BarcodeId { get; set; }
        [Required]
        public int SupplierID { get; set; }
        [ForeignKey("SupplierID")] 
        public Supplier Supplier { get; set; }
        [Required]
        public int TypeID { get; set; }
        [ForeignKey("TypeID")] 
        public TypeOfProduct TypeOfProduct { get; set; }
        [Required]
        public int ManufacturerID { get; set; }
        [ForeignKey("ManufacturerID")] 
        public Manufacturer Manufacturer { get; set; }
    }
}