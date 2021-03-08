using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaManagement.DataAccess.Data;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.DataAccess.DbInitialize
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {
                
            }

            if (_db.Roles.Any(r => r.Name == SD.Role_Admin)) return;
            if (_db.Roles.Any(r => r.Name == SD.Role_Staff)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin"
            },"Admin123@").GetAwaiter().GetResult() ;

            ApplicationUser userAdmin = _db.ApplicationUsers.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(userAdmin, SD.Role_Admin).GetAwaiter().GetResult();
            _userManager.CreateAsync(new StaffUser
            {
                UserName = "Staff@gmail.com",
                Email = "Staff@gmail.com",
                EmailConfirmed = true,
                Name = "Staff"
            },"Staff123@").GetAwaiter().GetResult() ;

            StaffUser userStaff = _db.StaffUsers.Where(u => u.Email == "Staff@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(userStaff, SD.Role_Staff).GetAwaiter().GetResult();
        }
    }
}