using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class UsersViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public StaffUser Staff { get; set; }
        public IEnumerable<SelectListItem> BranchList { get; set; }
    }
}