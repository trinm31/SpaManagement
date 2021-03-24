using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class ServiceUsers
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public DateTime ServedDate { get; set; }
        [Required] 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Note { get; set; }
        [Required] 
        public string StaffId { get; set; }
        [ForeignKey("StaffId")] 
        public StaffUser StaffUser { get; set; }
    }
}