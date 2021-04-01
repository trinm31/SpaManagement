using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.Utility.Enum;

namespace SpaManagement.DataAccess.DbInitialize
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
            var customerFromDb = _db.Customers.Where(c => c.id == 1);
            if (customerFromDb.Any())
            {
                return;
            }
            else
            {
                Customer customer = new Customer()
                {
                    IdentityCard = "000000000",
                    DateOfBirth = new DateTime(2021, 1, 1),
                    Phone = "0000000000",
                    Job = "Spa",
                    Sex = Gender.Other,
                    Name = "Spa"
                };
                _db.Customers.Add(customer);
                _db.SaveChanges();
            }

            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Staff)).GetAwaiter().GetResult();

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