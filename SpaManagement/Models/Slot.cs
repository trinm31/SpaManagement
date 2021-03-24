using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class Slot
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public int ServiceUserId { get; set; }
        [ForeignKey("ServiceUserId")] 
        public ServiceUsers ServiceUsers { get; set; }
        [Required] 
        public int ServiceDetailId { get; set; }
        [ForeignKey("ServiceDetailId")] 
        public ServiceDetail ServiceDetail { get; set; }
        [Required]
        public double Paid { get; set; }
    }
}