using System.ComponentModel.DataAnnotations.Schema;

namespace SpaManagement.Models
{
    public class StaffUser : ApplicationUser
    {
        public int BranchId { get; set; }
        [ForeignKey("BranchId")] 
        public Branch Branch { get; set; }
    }
}