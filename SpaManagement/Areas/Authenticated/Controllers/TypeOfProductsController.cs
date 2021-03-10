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
    public class TypeOfProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeOfProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            TypeOfProduct typeOfProduct = new TypeOfProduct();
            if (id == null)
            {
                return View(typeOfProduct);
            }

            typeOfProduct = await _unitOfWork.TypeOfProduct.GetAsync(id.GetValueOrDefault());
            if (typeOfProduct == null)
            {
                return NotFound();
            }
            return View(typeOfProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TypeOfProduct typeOfProduct)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.TypeOfProduct
                        .GetAllAsync(c => c.Name == typeOfProduct.Name && c.Id != typeOfProduct.Id);
                var typeCodeFromDb =
                    await _unitOfWork.TypeOfProduct
                        .GetAllAsync(c => c.TypeCode == typeOfProduct.TypeCode && c.Id != typeOfProduct.Id);
                if (typeOfProduct.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(typeOfProduct);
                    } 
                    else if (typeCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Type Code already exists";
                        return View(typeOfProduct);
                    }
                    else
                    {
                        await _unitOfWork.TypeOfProduct.AddAsync(typeOfProduct);
                    }
                }
                if (typeOfProduct.Id != 0 && !nameFromDb.Any() && !typeCodeFromDb.Any()) 
                {
                    if (nameFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Name already exists";
                        return View(typeOfProduct);
                    } 
                    else if (typeCodeFromDb.Any())
                    {
                        ViewData["Message"] = "Error: Type Code already exists";
                        return View(typeOfProduct);
                    }
                    else
                    {
                        await _unitOfWork.TypeOfProduct.Update(typeOfProduct);
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfProduct);
        }
    }
}