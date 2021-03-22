using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerId;
        private static List<ServiceDetailsViewmodel> _serviceList = new List<ServiceDetailsViewmodel>();
        public ServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Service(int id)
        {
            if (id != 0)
            {
                _customerId = id;
            }
            return View();
        }
        

        public async Task<IActionResult> Details(int id)
        {
            var service = await _unitOfWork.CategoryService.GetAsync(id);
            if (service == null)
            {
                return RedirectToAction(nameof(service));
            }

            ServiceDetailsViewmodel serviceDetails = new ServiceDetailsViewmodel()
            {
                Service = service
            };
            return View(serviceDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ServiceDetailsViewmodel serviceDetails)
        {
            serviceDetails.Service = await _unitOfWork.CategoryService.GetAsync(serviceDetails.Service.Id);
            var isServiceExist = _serviceList.FindAll(s => s.Service.Id == serviceDetails.Service.Id);
            if (isServiceExist.Any())
            {
                var service = _serviceList.Find(s => s.Service.Id == serviceDetails.Service.Id);
                service.Slot += serviceDetails.Slot;
                _serviceList.Remove(service);
                _serviceList.Add(service);
            }
            else
            {
                _serviceList.Add(serviceDetails);
            }
            return RedirectToAction(nameof(Service));
        }
        public async Task<IActionResult> Summary()
        {
            IEnumerable<ServiceDetailsViewmodel> serviceDetailsViewmodels = _serviceList;
            return View(serviceDetailsViewmodels);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var currentUser = await _unitOfWork.Staff.GetAsync(claims.Value);
            var branchId = currentUser.BranchId;
            var customerId = _customerId;
            return View();
            //return RedirectToAction(nameof(OrderSummary), new {customerId = customerId, branchId = branchId});
        }
        // public async Task<IActionResult> OrderSummary(int branchId, int customerId)
        // {
        //     double price = 0;
        //     foreach (var service in _serviceList)
        //     {
        //         price += service.Slot * service.Service.Price;
        //     }
        //     SoldOrderSummaryViewModel soldOrderSummaryViewModel = new SoldOrderSummaryViewModel()
        //     {
        //         BranchId = branchId,
        //         CustomerId = customerId,
        //         //ProductList = _productList,
        //         ProductDetails = new ProductDetail(),
        //         Order = new Order(),
        //         Price = price
        //     };
        //     soldOrderSummaryViewModel.Order.OrderDate = DateTime.Today;
        //     soldOrderSummaryViewModel.Order.OrderType = OrderType.Sold;
        //     return View(soldOrderSummaryViewModel);
        // }
        public IActionResult Plus(int cartId)
        {
            var service = _serviceList.Find(p => p.Service.Id == cartId);
            service.Slot += 1;
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Minus(int cartId)
        {
            var service = _serviceList.Find(p => p.Service.Id == cartId);
            if (service.Slot == 1)
            {
                _serviceList.Remove(service);
            }
            else
            {
                service.Slot -= 1;
            }
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Remove(int cartId)
        {
            var service = _serviceList.Find(p => p.Service.Id == cartId);
            _serviceList.Remove(service);
            return RedirectToAction(nameof(Summary));
        }
    }
}