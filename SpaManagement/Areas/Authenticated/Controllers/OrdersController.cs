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
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                CustomersList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                Order = new Order()
            };
            if (id == null)
            {
                return View(orderViewModel);
            }

            orderViewModel.Order = await _unitOfWork.Order.GetAsync(id.GetValueOrDefault());
            if (orderViewModel.Order == null)
            {
                return NotFound();
            }
            return View(orderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(OrderViewModel orderViewModel)
        {
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            if (ModelState.IsValid)
            {
                if (orderViewModel.Order.Id == 0)
                {
                    await _unitOfWork.Order.AddAsync(orderViewModel.Order);
                }
               
                if (orderViewModel.Order.Id  != 0) 
                {
                    await _unitOfWork.Order.Update(orderViewModel.Order);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            orderViewModel = new OrderViewModel()
            {
                CustomersList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                Order = new Order()
            };
            return View(orderViewModel);
        }
    }
}