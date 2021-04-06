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
    public class SuppliersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuppliersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Supplier supplier = new Supplier();
            if (id == null)
            {
                return View(supplier);
            }

            supplier = await _unitOfWork.Supplier.GetAsync(id.GetValueOrDefault());
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.Supplier
                        .GetAllAsync(c => c.Name == supplier.Name && c.Id != supplier.Id);
                var suppliersCodeFromDb =
                    await _unitOfWork.Supplier
                        .GetAllAsync(c => c.SupplierCode == supplier.SupplierCode && c.Id != supplier.Id);
                if (supplier.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(supplier);
                    } 
                    else if (suppliersCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Supplier Code already exists";
                        return View(supplier);
                    }
                    else
                    {
                        await _unitOfWork.Supplier.AddAsync(supplier);
                        await notificationTask("Supplier", $"Add {supplier.Name}");
                    }

                }

                if (supplier.Id != 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(supplier);
                    }
                    else if (suppliersCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Supplier Code already exists";
                        return View(supplier);
                    }
                    else
                    {
                        await _unitOfWork.Supplier.Update(supplier);
                        await notificationTask("Supplier", $"Update {supplier.Name}");
                    }
                }
                
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
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