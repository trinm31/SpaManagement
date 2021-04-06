using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class ServiceUseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerId;
        private static int _serviceId;

        public ServiceUseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Customer"] = TempData["Customer"];
            ViewData["Service"] = TempData["Service"];
            ViewData["Form"] = TempData["Form"];
            IEnumerable<StaffUser> staffUsers = await _unitOfWork.Staff.GetAllAsync();
            ServiceUseViewModel serviceUseViewModel = new ServiceUseViewModel()
            {
                StaffList = staffUsers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
            };
            return View(serviceUseViewModel);
        }
        public async Task<IActionResult> Choose(int id)
        {
            if (id != 0)
            {
                _customerId = id;
            }

            var customer = await _unitOfWork.Customer.GetAsync(id);
            TempData["Customer"] = $"Success: Customer {customer.Name} is chosen";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ServiceDetail(int id)
        {
            if (id != 0)
            {
                _serviceId = id;
            }
            TempData["Service"] = $"Success: Service {id} is chosen";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceUse(ServiceUseViewModel serviceUseViewModel)
        {
            var serviceDetail = await _unitOfWork.ServiceDetail.GetAsync(_serviceId);
            var isSlotExist = await _unitOfWork.Slot.GetAllAsync(s => s.ServiceDetailId == _serviceId);
            if (serviceDetail.Slot > isSlotExist.Count())
            {
                ServiceUsers serviceUsers = new ServiceUsers()
                {
                    ServedDate = DateTime.Today,
                    StartTime = serviceUseViewModel.StartTime,
                    EndTime = serviceUseViewModel.EndTime,
                    Note = serviceUseViewModel.Note,
                    StaffId = serviceUseViewModel.StaffId
                };
                await _unitOfWork.ServiceUsers.AddAsync(serviceUsers);
                _unitOfWork.Save();
                await notificationTask("ServiceUse", $"Add Service User With id {serviceUsers.Id}");
                Slot slot = new Slot()
                {
                    ServiceUserId = serviceUsers.Id,
                    ServiceDetailId = serviceDetail.Id,
                    Paid = serviceUseViewModel.Paid
                };
                await _unitOfWork.Slot.AddAsync(slot);
                await notificationTask("ServiceUse", $"Add Slot With id {slot.Id}");
                if (serviceDetail.Debt != 0)
                {
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Credit = serviceUseViewModel.Paid,
                        CustomerId = _customerId,
                        ServiceDetailId = serviceDetail.Id
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    await notificationTask("ServiceUse", $"Add Account With id {account.Id}");
                }
                _unitOfWork.Save();
                _customerId = 0;
                _serviceId = 0;
                TempData["Form"] = $"Success: Operation successfully";
            }
            else
            {
                TempData["Form"] = $"Error: No more slots";
            }
            return RedirectToAction(nameof(Index));
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _unitOfWork.Customer.GetAllAsync(c=> c.id != 1);
            return Json(new { data = customerList });
        }
        [HttpGet]
        public async Task<IActionResult> GetService()
        {
            var servicelist = 
                await _unitOfWork.ServiceDetail.GetAllAsync(i=>
                    i.CustomerId ==_customerId,includeProperties:"CategoryService");
            return Json(new { data = servicelist });
        }
        #endregion
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