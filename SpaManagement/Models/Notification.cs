using System;
using System.ComponentModel.DataAnnotations;
using SpaManagement.Utility.Enum;

namespace SpaManagement.Models
{
    public class Notification
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required] 
        public NotificationStatus Status { get; set; }
    }
}