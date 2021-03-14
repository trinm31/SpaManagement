using System;
using System.Collections;
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
        public IActionResult Customer()
        {
            return View();
        }
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _unitOfWork.Customer.GetAllAsync();
            return Json(new { data = customerList });
        }

        public async Task<IActionResult> Order(int id)
        {
            var customer = await _unitOfWork.Customer.GetAsync(id);
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.Order = new Order();
            orderViewModel.Product = new Product();
            orderViewModel.Order.CustomerId = customer.id;
            orderViewModel.Order.OrderDate = DateTime.Today;
            var products = await _unitOfWork.Product.GetAllAsync();
            orderViewModel.Products = products.Select(I => new SelectListItem
            {
                Text = I.Name,
                Value = I.Id.ToString()
            });
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderViewModel orderViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var staffUser =
                await _unitOfWork.Staff.GetFirstOrDefaultAsync(i => i.Id == claim.Value, includeProperties: "Branch");
            var branchID = staffUser.BranchId;
            var productDetailsID = await _unitOfWork.ProductDetail.GetFirstOrDefaultAsync(i =>
                i.ProductID == orderViewModel.Product.Id && i.BranchID == branchID);
            Models.Order order = new Order();
            order.Amount = orderViewModel.Price * orderViewModel.Quantity;
            order.OrderDate = DateTime.Today;
            order.Debt = order.Amount - orderViewModel.Order.PaidAmount;
            order.Note = orderViewModel.Order.Note;
            order.CustomerId = orderViewModel.Order.CustomerId;
            order.OrderType = orderViewModel.Order.OrderType;
            order.PaidAmount = orderViewModel.Order.PaidAmount;
            await _unitOfWork.Order.AddAsync(order);
            OrderDetail orderDetail = new OrderDetail()
            {
                Price = orderViewModel.Price,
                Quantity = orderViewModel.Quantity,
                Discount = orderViewModel.Discount,
                OrderID = orderViewModel.Order.Id,
                ProductDetailId = productDetailsID.Id
            };
            await _unitOfWork.OrderDetail.AddAsync(orderDetail);
            var accountDb = await 
                _unitOfWork.Account.GetAllAsync(i => i.CustomerId == orderViewModel.Order.CustomerId);
            if (accountDb.Any())
            {
                var accountUpdate = accountDb.FirstOrDefault();
                accountUpdate.Credit += orderViewModel.Order.PaidAmount;
                accountUpdate.Debt += orderViewModel.Order.Debt;
                //accountUpdate.Amount += orderViewModel.Order.Amount;
            }
            else
            {
                Account account = new Account()
                {
                    //CustomerId = orderViewModel.Order.CustomerId,
                    TransactDate = order.OrderDate,
                    Credit = orderViewModel.Order.PaidAmount,
                    Debt = order.Amount - orderViewModel.Order.PaidAmount,
                    //Amount = order.Amount
                };
                await _unitOfWork.Account.AddAsync(account);
            }

            _unitOfWork.Save();
            var customeridTemp = orderViewModel.Order.CustomerId;
            orderViewModel = new OrderViewModel();
            orderViewModel.Order = new Order();
            orderViewModel.Product = new Product();
            orderViewModel.Order.CustomerId = customeridTemp;
            orderViewModel.Order.OrderDate = DateTime.Today;
            var products = await _unitOfWork.Product.GetAllAsync();
            orderViewModel.Products = products.Select(I => new SelectListItem
            {
                Text = I.Name,
                Value = I.Id.ToString()
            });
            return View(orderViewModel);
        }
    }
}