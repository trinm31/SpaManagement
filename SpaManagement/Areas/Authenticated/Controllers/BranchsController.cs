using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BranchsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Branch branch = new Branch();
            if (id == null)
            {
                return View(branch);
            }

            branch = await _unitOfWork.Branch.GetAsync(id.GetValueOrDefault());
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Branch branch)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.Branch
                        .GetAllAsync(c => c.Name == branch.Name && c.Id != branch.Id);
                var branchCodeFromDb =
                    await _unitOfWork.Branch
                        .GetAllAsync(c => c.BranchCode == branch.BranchCode && c.Id != branch.Id);
                if (branch.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(branch);
                    } 
                    else if (branchCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Branch Code already exists";
                        return View(branch);
                    }
                    else
                    {
                        await _unitOfWork.Branch.AddAsync(branch);
                        await notificationTask("Branch",$"Add {branch.Name}");
                    }
                    
                }
               
                if (branch.Id != 0) 
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(branch);
                    } 
                    else if (branchCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Branch Code already exists";
                        return View(branch);
                    }
                    else
                    {
                        await _unitOfWork.Branch.Update(branch);
                        await notificationTask("Branch",$"Update {branch.Name}");
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
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