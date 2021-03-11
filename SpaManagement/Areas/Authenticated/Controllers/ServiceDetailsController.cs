using System.Collections;
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
    [Authorize(Roles = SD.Role_Admin)]
    public class ServiceDetailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<CategoryService> categoryServices = await _unitOfWork.CategoryService.GetAllAsync();
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            ServiceDetailViewModel serviceDetailViewModel = new ServiceDetailViewModel()
            {
                CategoryServiceList = categoryServices.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                CustomerList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                ServiceDetail = new ServiceDetail()
            };
            if (id == null)
            {
                return View(serviceDetailViewModel);
            }

            serviceDetailViewModel.ServiceDetail = await _unitOfWork.ServiceDetail.GetAsync(id.GetValueOrDefault());
            if (serviceDetailViewModel.ServiceDetail == null)
            {
                return NotFound();
            }
            return View(serviceDetailViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ServiceDetailViewModel serviceDetailViewModel)
        {
            IEnumerable<CategoryService> categoryServices = await _unitOfWork.CategoryService.GetAllAsync();
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            if (ModelState.IsValid)
            {
                var IsServiceDetailsExited =
                    await _unitOfWork.ServiceDetail
                        .GetAllAsync(i => i.CustomerId == serviceDetailViewModel.ServiceDetail.ServiceId
                                          && i.ServiceId == serviceDetailViewModel.ServiceDetail.ServiceId);
                if (serviceDetailViewModel.ServiceDetail.Id == 0)
                {
                    if (IsServiceDetailsExited.Any())
                    {
                        serviceDetailViewModel = new ServiceDetailViewModel()
                        {
                            CategoryServiceList = categoryServices.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            CustomerList = customers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.id.ToString()
                            }),
                            ServiceDetail = new ServiceDetail()
                        };
                        ViewData["Message"] = "Error: Quantity for this product in this branch already exists";
                        return View(serviceDetailViewModel);
                    }
                    else
                    {
                        await _unitOfWork.ServiceDetail.AddAsync(serviceDetailViewModel.ServiceDetail);
                    }
                    
                }
               
                if (serviceDetailViewModel.ServiceDetail.Id != 0) 
                {
                    if (IsServiceDetailsExited.Any())
                    {
                        serviceDetailViewModel = new ServiceDetailViewModel()
                        {
                            CategoryServiceList = categoryServices.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            CustomerList = customers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.id.ToString()
                            }),
                            ServiceDetail = new ServiceDetail()
                        };
                        ViewData["Message"] = "Error: Name already exists";
                        return View(serviceDetailViewModel);
                    }
                    else
                    {
                        await _unitOfWork.ServiceDetail.Update(serviceDetailViewModel.ServiceDetail);
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            serviceDetailViewModel = new ServiceDetailViewModel()
            {
                CategoryServiceList = categoryServices.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                CustomerList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                ServiceDetail = new ServiceDetail()
            };
            return View(serviceDetailViewModel);
        }
    }
}