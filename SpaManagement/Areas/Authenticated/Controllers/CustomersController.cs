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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
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
                    await notificationTask("Customer",$"Add {customer.Name}");
                }
                else if (customer.id != 0 && !IdentityromDb.Any()) 
                {
                    await _unitOfWork.Customer.Update(customer);
                    await notificationTask("Customer",$"Update {customer.Name}");
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