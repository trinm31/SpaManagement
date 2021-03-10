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
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Customer customer = new Customer();
            if (id == null)
            {
                return View(customer);
            }

            customer = await _unitOfWork.Customer.GetAsync(id.GetValueOrDefault());
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var IdentityromDb =
                    await _unitOfWork.Customer
                        .GetAllAsync(c => c.IdentityCard == customer.IdentityCard && c.id != customer.id);
                if (customer.id == 0 && !IdentityromDb.Any())
                {
                    await _unitOfWork.Customer.AddAsync(customer);
                }
                else if (customer.id != 0 && !IdentityromDb.Any()) 
                {
                    await _unitOfWork.Customer.Update(customer);
                }
                else
                {
                    ViewData["Message"] = "Error: Identity already exists";
                    return View(customer);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}