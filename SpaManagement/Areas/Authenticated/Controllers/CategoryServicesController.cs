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
    public class CategoryServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            CategoryService categoryService = new CategoryService();
            if (id == null)
            {
                return View(categoryService);
            }

            categoryService = await _unitOfWork.CategoryService.GetAsync(id.GetValueOrDefault());
            if (categoryService == null)
            {
                return NotFound();
            }
            return View(categoryService);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CategoryService categoryService)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.CategoryService
                        .GetAllAsync(c => c.Name == categoryService.Name && c.Id != categoryService.Id);
                var categoryServiceFromDb =
                    await _unitOfWork.CategoryService
                        .GetAllAsync(c => c.ServiceCode == categoryService.ServiceCode && c.Id != categoryService.Id);
                if (categoryService.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(categoryService);
                    } 
                    else if (categoryServiceFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Service Code already exists";
                        return View(categoryService);
                    }
                    else
                    {
                        await _unitOfWork.CategoryService.AddAsync(categoryService);
                        await notificationTask("CategoryService",$"Add {categoryService.Name}");
                    }
                    
                }
               
                if (categoryService.Id != 0) 
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(categoryService);
                    } 
                    else if (categoryServiceFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Service Code already exists";
                        return View(categoryService);
                    }
                    else
                    {
                        await _unitOfWork.CategoryService.Update(categoryService);
                        await notificationTask("CategoryService",$"Update {categoryService.Name}");
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryService);
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