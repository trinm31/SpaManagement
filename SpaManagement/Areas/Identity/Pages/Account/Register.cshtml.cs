using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string PhoneNumber { get; set; }
            [Required]
            public int Branch { get; set; }
            public IEnumerable<SelectListItem> BranchList { get; set; }
            [Required]
            public string Role { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (User.IsInRole(SD.Role_Admin))
            {
                IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
                Input = new InputModel()
                {
                    RoleList = _roleManager.Roles.Where(x=> x.Name != SD.Role_Customer).Select(x=> x.Name).Select(i=> new SelectListItem
                    {
                        Text = i,
                        Value = i
                    }),
                    BranchList = branchList.Select(i=> new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
            }

            if (User.IsInRole(SD.Role_Staff))
            {
                IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
                Input = new InputModel()
                {
                    RoleList = _roleManager.Roles.Where(x=> x.Name == SD.Role_Customer).Select(x=> x.Name).Select(i=> new SelectListItem
                    {
                        Text = i,
                        Value = i
                    }),
                    BranchList = branchList.Select(i=> new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                StaffUser staffUser = new StaffUser();
                Customer customer = new Customer();
                IdentityResult result = new IdentityResult();
                if (Input.Role == SD.Role_Customer)
                {
                    customer = new Customer()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Role = Input.Role,
                        Name = Input.Name
                    };
                    result = await _userManager.CreateAsync(customer, Input.Password);
                    
                }else if (Input.Role == SD.Role_Staff)
                {
                    staffUser = new StaffUser()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Role = Input.Role,
                        Name = Input.Name,
                        BranchId = Input.Branch
                    };
                    result = await _userManager.CreateAsync(staffUser, Input.Password);
                }
                else
                {
                    applicationUser = new ApplicationUser()
                    {
                        UserName = Input.Email, 
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        Role = Input.Role,
                        Name = Input.Name
                    };
                    result = await _userManager.CreateAsync(applicationUser, Input.Password);
                }
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (User.IsInRole(SD.Role_Customer))
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(customer);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = customer.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }else if (User.IsInRole(SD.Role_Staff))
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(staffUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = staffUser.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    else
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = applicationUser.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (User.IsInRole(SD.Role_Customer))
                        {
                            await _signInManager.SignInAsync(customer, isPersistent: false);
                        }else if (User.IsInRole(SD.Role_Staff))
                        {
                            await _signInManager.SignInAsync(staffUser, isPersistent: false);
                        }
                        else
                        {
                            await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                        }
                        
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
