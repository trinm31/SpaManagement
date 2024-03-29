using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
   
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ForgotPassword(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetAsync(id);

            if (user == null)
            {
                return View();
            }

            ForgotPasswordViewModel UserEmail = new ForgotPasswordViewModel()
            {
                Email = user.Email
            };
            return View(UserEmail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    return RedirectToAction("ResetPassword", "Users", new {email = model.Email, token = token});
                }

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    else
                    {
                        ViewData["Message"] = "Error: Your Password not permitted";
                        return View(model);
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    await notificationTask("User", $"Reset password userid {user.Id}");
                    return View(model);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetAsync(id);

            if (user == null)
            {
                return View();
            }

            var usertemp = await _userManager.FindByIdAsync(user.Id);
            var roleTemp = await _userManager.GetRolesAsync(usertemp);
            user.Role = roleTemp.FirstOrDefault();
            IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
            UsersViewModel usersVm = new UsersViewModel();
            usersVm.BranchList = branchList.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
             
            if (user.Role == SD.Role_Staff)
            {
                var staffUser = await _unitOfWork.Staff.GetAsync(id);
                usersVm.Staff = staffUser;
            }
            else
            {
                usersVm.ApplicationUser = user;
            }

            return View(usersVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersViewModel user)
        {
            UsersViewModel usersVm = new UsersViewModel();
            if (user.ApplicationUser != null)
            {
                usersVm.ApplicationUser = user.ApplicationUser;
                var applicationUsers =
                    await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id == user.ApplicationUser.Id);
                var userEmailDb =
                    await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Email == user.ApplicationUser.Email);
                if (!userEmailDb.Any() && !applicationUsers.Any())
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
                var applicationUser = await _unitOfWork.ApplicationUser.GetAsync(user.ApplicationUser.Id);
                applicationUser.Name = user.ApplicationUser.Name;
                await _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
                await notificationTask("User", $"Update userid {applicationUser.Id}");
                return RedirectToAction(nameof(Index));
            }
            if (user.Staff != null)
            {
                usersVm.Staff = user.Staff;
                var staffUsers =
                    await _unitOfWork.Staff.GetAllAsync(u => u.Id == user.Staff.Id);
                var userEmailDb =
                    await _unitOfWork.Staff.GetAllAsync(u => u.Email == user.Staff.Email);
                var profile = await _unitOfWork.Staff.GetAsync(user.Staff.Id);
                if (!userEmailDb.Any() && !staffUsers.Any())
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
                profile.Name = user.Staff.Name;
                profile.BranchId = user.Staff.BranchId;
                profile.PhoneNumber = user.Staff.PhoneNumber;
                await _unitOfWork.Staff.Update(profile);
                _unitOfWork.Save();
                await notificationTask("User", $"Update userid {profile.Id}");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        private async Task notificationTask(string controller, string action = null)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userDb = await _unitOfWork.ApplicationUser.GetAsync(claims.Value);
            string Notimessage = $"User {userDb.Name} delete {controller} for {action}";
            Notification notification = new Notification()
            {
                Date = DateTime.Today,
                Content = Notimessage
            };
            await _unitOfWork.Notification.AddAsync(notification);
            _unitOfWork.Save();
        }
    }
}