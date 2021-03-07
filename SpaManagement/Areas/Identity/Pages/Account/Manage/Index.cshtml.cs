using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Name { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var usertemp = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claims.Value);
            var role = await _userManager.GetRolesAsync(usertemp);

            Username = userName;

            if (role.FirstOrDefault() == SD.Role_Staff)
            {
                var userFromDb = await _unitOfWork.Staff.GetAsync(user.Id);
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = userFromDb.Name,
                };
            }
            if (role.FirstOrDefault() == SD.Role_Admin)
            {
                var userFromDb = await _unitOfWork.ApplicationUser.GetAsync(user.Id);
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Name = userFromDb.Name,
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (User.IsInRole(SD.Role_Staff))
            {
                var profile = await _unitOfWork.Staff.GetAsync(user.Id);
                if (Input.PhoneNumber != profile.PhoneNumber)
                {
                    profile.PhoneNumber = Input.PhoneNumber;
                }

                if (Input.Name != profile.Name)
                {
                    profile.Name = Input.Name;
                }
                await _unitOfWork.Staff.Update(profile);
                _unitOfWork.Save();
            }
            if (User.IsInRole(SD.Role_Admin))
            {
                var profile = await _unitOfWork.ApplicationUser.GetAsync(user.Id);
                if (Input.PhoneNumber != profile.PhoneNumber)
                {
                    profile.PhoneNumber = Input.PhoneNumber;
                }

                if (Input.Name != profile.Name)
                {
                    profile.Name = Input.Name;
                }
                await _unitOfWork.ApplicationUser.Update(profile);
                _unitOfWork.Save();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
