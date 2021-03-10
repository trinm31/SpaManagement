using System.Linq;
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
    public class ManufacturersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManufacturersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Manufacturer manufacturer = new Manufacturer();
            if (id == null)
            {
                return View(manufacturer);
            }

            manufacturer = await _unitOfWork.Manufacturer.GetAsync(id.GetValueOrDefault());
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.Manufacturer
                        .GetAllAsync(c => c.Name == manufacturer.Name && c.Id != manufacturer.Id);
                var manufacturersCodeFromDb =
                    await _unitOfWork.Manufacturer
                        .GetAllAsync(c => c.ManufacturerCode == manufacturer.ManufacturerCode && c.Id != manufacturer.Id);
                if (manufacturer.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(manufacturer);
                    } 
                    else if (manufacturersCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Manufacturer Code already exists";
                        return View(manufacturer);
                    }
                    else
                    {
                        await _unitOfWork.Manufacturer.AddAsync(manufacturer);
                    }
                }
                if (manufacturer.Id != 0) 
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(manufacturer);
                    } 
                    else if (manufacturersCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Manufacturer Code already exists";
                        return View(manufacturer);
                    }
                    else
                    {
                        await _unitOfWork.Manufacturer.Update(manufacturer);
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }
    }
}