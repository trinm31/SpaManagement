using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.ReportModel;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
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
            ViewData["Customer"] = TempData["Customer"];
            ViewData["Service"] = TempData["Service"];
            IEnumerable<ServiceDetailsViewmodel> serviceDetailsViewmodels = _serviceList;
            return View(serviceDetailsViewmodels);
        }
        
        public async Task<IActionResult> Choose(int id)
        {
            if (id != 0)
            {
                _customerId = id;
                _serviceList.Clear();
            }

            var customer = await _unitOfWork.Customer.GetAsync(id);
            TempData["Customer"] = $"Success: Customer {customer.Name} is chosen";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddService(int id)
        {
            var service = await _unitOfWork.CategoryService.GetAsync(id);
            if (service == null)
            {
                TempData["Service"] = $"Error: Service is null";
                return RedirectToAction(nameof(Index));
            }

            ServiceDetailsViewmodel serviceDetails = new ServiceDetailsViewmodel()
            {
                Service = service,
                Slot = 1
            };
            var isServiceExist = _serviceList.FindAll(s => s.Service.Id == service.Id);
            if (isServiceExist.Any())
            {
                var servicetemp = _serviceList.Find(s => s.Service.Id == service.Id);
                servicetemp.Slot += serviceDetails.Slot;
                _serviceList.Remove(servicetemp);
                _serviceList.Add(servicetemp);
                TempData["Service"] = $"Success: Service is add more";
            }
            else
            {
                _serviceList.Add(serviceDetails);
                TempData["Service"] = $"Success: Service is added";
            }
            return RedirectToAction(nameof(Index));
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            var customerId = _customerId;
            return RedirectToAction(nameof(OrderSummary), new {customerId = customerId});
        }
        public async Task<IActionResult> OrderSummary(int customerId)
        {
            double price = 0;
            foreach (var service in _serviceList)
            {
                price += service.Slot * service.Service.Price * (1-service.Service.Discount);
            }

            ServiceOrderSummaryViewModel serviceOrderSummaryViewModel = new ServiceOrderSummaryViewModel()
            {
                CustomerId = _customerId,
                ServiceList = _serviceList,
                Price = price
            };
            return View(serviceOrderSummaryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderSummary(ServiceOrderSummaryViewModel serviceOrderSummaryViewModel)
        {
            double price = 0;
            foreach (var service in _serviceList)
            {
                price += service.Slot * service.Service.Price * (1-service.Service.Discount);
            }

            for (int i = 0; i < _serviceList.Count(); i++)
            {
                ServiceDetail serviceDetail = new ServiceDetail()
                {
                    OrderDate = DateTime.Today,
                    CustomerId = _customerId,
                    ServiceId = _serviceList[i].Service.Id,
                    Price = _serviceList[i].Slot*_serviceList[i].Service.Price*(1-_serviceList[i].Service.Discount),
                    Slot = _serviceList[i].Slot
                };
                await _unitOfWork.ServiceDetail.AddAsync(serviceDetail);
                _unitOfWork.Save();
                await notificationTask("Service", $"Add Service Detail with Id {serviceDetail.Id}");
                if (serviceOrderSummaryViewModel.PaidAmount>= serviceDetail.Price)
                {
                    serviceDetail.Paid = serviceDetail.Price;
                    serviceDetail.Debt = 0;
                    serviceOrderSummaryViewModel.PaidAmount =
                        serviceOrderSummaryViewModel.PaidAmount - serviceDetail.Price;
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Debt = serviceDetail.Debt,
                        Credit = serviceDetail.Paid,
                        CustomerId = _customerId,
                        ServiceDetailId = serviceDetail.Id
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    await _unitOfWork.ServiceDetail.Update(serviceDetail);
                    await notificationTask("Service", $"Add Account with Id {account.Id}");
                }
                else
                {
                    serviceDetail.Paid = serviceOrderSummaryViewModel.PaidAmount;
                    serviceDetail.Debt = Math.Abs(serviceDetail.Price - serviceDetail.Paid);
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Debt = serviceDetail.Debt,
                        Credit = serviceDetail.Paid,
                        CustomerId = _customerId,
                        ServiceDetailId = serviceDetail.Id
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    await _unitOfWork.ServiceDetail.Update(serviceDetail);
                    await notificationTask("Service", $"Add Account with Id {account.Id}");
                }
                
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Invoice));
        }
        public IActionResult Plus(int cartId)
        {
            var service = _serviceList.Find(p => p.Service.Id == cartId);
            service.Slot += 1;
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Remove(int cartId)
        {
            var service = _serviceList.Find(p => p.Service.Id == cartId);
            _serviceList.Remove(service);
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        private async Task notificationTask(string controller, string action = null)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
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
        [HttpGet] 
        public async Task<IActionResult> Invoice()
        {
            var customerDb = await _unitOfWork.Customer.GetAsync(_customerId);
            ServiceInvoice invoice = new ServiceInvoice()
            {
                CustomerName = customerDb.Name,
                IdentityCard = customerDb.IdentityCard,
                Phone = customerDb.Phone,
                servicelist = _serviceList,
            };
            _customerId = 0;
            return new ViewAsPdf("InvoicePDF", invoice);
        }
    }
}