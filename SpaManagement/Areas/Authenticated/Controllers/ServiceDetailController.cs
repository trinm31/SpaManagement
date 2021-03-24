using System.Collections.Generic;
using System.Linq;
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
    public class ServiceDetailController: Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerId;
        private static int _serviceId;
        private static int _serviceUserId;
        public ServiceDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Customer"] = TempData["Customer"];
            ViewData["Service"] = TempData["Service"];
            ViewData["Form"] = TempData["Form"];
            IEnumerable<StaffUser> staffUsers = await _unitOfWork.Staff.GetAllAsync();
            ServiceDetailViewModel serviceDetailViewModel = new ServiceDetailViewModel()
            {
                ServiceDetail = new ServiceDetail(),
                ServiceUsers = new ServiceUsers(),
                StaffList = staffUsers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
            };
            if (_serviceId != 0 && _serviceUserId == 0)
                {
                    var serviceDetail = await _unitOfWork.ServiceDetail.GetAsync(_serviceId);
                    serviceDetailViewModel = new ServiceDetailViewModel()
                    {
                        ServiceDetail = serviceDetail,
                        ServiceUsers = new ServiceUsers(),
                        StaffList = staffUsers.Select(I => new SelectListItem
                        {
                            Text = I.Name,
                            Value = I.Id.ToString()
                        }),
                    };
                    return View(serviceDetailViewModel);
                }

            if (_serviceUserId != 0)
            {
                var slot = await _unitOfWork.Slot.GetFirstOrDefaultAsync(s=> s.ServiceDetailId == _serviceId && s.ServiceUserId == _serviceUserId);
                var serviceUser = await _unitOfWork.ServiceUsers.GetAsync(_serviceUserId);
                serviceDetailViewModel = new ServiceDetailViewModel()
                {
                    ServiceDetail = new ServiceDetail(),
                    Slot = slot,
                    ServiceUsers = serviceUser,
                    StaffList = staffUsers.Select(I => new SelectListItem
                    {
                        Text = I.Name,
                        Value = I.Id.ToString()
                    }),
                };
                return View(serviceDetailViewModel);
            }
            return View(serviceDetailViewModel);
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
        public async Task<IActionResult> ServiceUser(int id)
        {
            if (id != 0)
            {
                _serviceUserId = id;
            }
            TempData["Service"] = $"Success: Slot {id} is chosen";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServiceDetail(ServiceDetailViewModel serviceDetailViewModel)
        {
            var serviceDetailDb = await _unitOfWork.ServiceDetail.GetAsync(_serviceId);
            serviceDetailDb.Slot = serviceDetailViewModel.ServiceDetail.Slot;
            serviceDetailDb.Paid = serviceDetailViewModel.ServiceDetail.Paid;
            serviceDetailDb.Price = serviceDetailViewModel.ServiceDetail.Price;
            await _unitOfWork.ServiceDetail.Update(serviceDetailDb);
            _unitOfWork.Save();
            _serviceId = 0;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlot(ServiceDetailViewModel serviceDetailViewModel)
        {
            var ServiceUser = await _unitOfWork.ServiceUsers.GetAsync(_serviceUserId);
            ServiceUser.Note = serviceDetailViewModel.ServiceUsers.Note;
            ServiceUser.StartTime = serviceDetailViewModel.ServiceUsers.StartTime;
            ServiceUser.EndTime = serviceDetailViewModel.ServiceUsers.EndTime;
            ServiceUser.StaffId = serviceDetailViewModel.ServiceUsers.StaffId;
            await _unitOfWork.ServiceUsers.AddAsync(ServiceUser);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region API
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _unitOfWork.Customer.GetAllAsync();
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
        [HttpGet]
        public async Task<IActionResult> GetServiceUser()
        {
            var slotList = 
                await _unitOfWork.Slot.GetAllAsync(i=>
                    i.ServiceDetailId ==_serviceId,includeProperties:"ServiceDetail");
            return Json(new { data = slotList });
        }
        #endregion
    }
}