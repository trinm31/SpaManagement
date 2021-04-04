using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.Utility.Enum;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class SoldController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private static List<SoldDetailViewModel> _productList = new List<SoldDetailViewModel>();
        private static int _customerID;

        public SoldController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product(int id)
        {
            if (id != 0)
            {
                _customerID = id;
            }

            IEnumerable<SoldDetailViewModel> soldDetailViewModels = _productList;
            return View(soldDetailViewModels);
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }
        
        public async Task<IActionResult> AddItem(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(id);
            if (product == null)
            {
                TempData["Product"] = $"Error: Product is null";
                return RedirectToAction(nameof(Index));
            }

            SoldDetailViewModel soldDetailViewModel = new SoldDetailViewModel()
            {
                Product = product,
                Count = 1
            };
            var isproductExist = _productList.FindAll(s => s.Product.Id == product.Id);
            if (isproductExist.Any())
            {
                var productTemp = _productList.Find(s => s.Product.Id == product.Id);
                productTemp.Count += soldDetailViewModel.Count;
                _productList.Remove(productTemp);
                _productList.Add(productTemp);
                TempData["Product"] = $"Success: Product is add more";
            }
            else
            {
                _productList.Add(soldDetailViewModel);
                TempData["Product"] = $"Success: Product is added";
            }
            return RedirectToAction(nameof(Product));
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
            var customerId = _customerID;
            return RedirectToAction(nameof(OrderSummary), new {customerId = customerId, branchId = branchId});
        }

        public async Task<IActionResult> OrderSummary(int branchId, int customerId)
        {
            double price = 0;
            foreach (var product in _productList)
            {
                price += product.Count * product.Product.Price;
            }
            SoldOrderSummaryViewModel soldOrderSummaryViewModel = new SoldOrderSummaryViewModel()
            {
                BranchId = branchId,
                CustomerId = customerId,
                ProductList = _productList,
                ProductDetails = new ProductDetail(),
                Order = new Order(),
                Price = price
            };
            soldOrderSummaryViewModel.Order.OrderDate = DateTime.Today;
            soldOrderSummaryViewModel.Order.OrderType = OrderType.Sold;
            return View(soldOrderSummaryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderSummary(SoldOrderSummaryViewModel soldOrderSummaryViewModel)
        {
            soldOrderSummaryViewModel.Order.PaidAmount = soldOrderSummaryViewModel.PaidAmount;
            soldOrderSummaryViewModel.Order.Note = soldOrderSummaryViewModel.Note;
            soldOrderSummaryViewModel.Order.CustomerId = soldOrderSummaryViewModel.CustomerId;
            await _unitOfWork.Order.AddAsync(soldOrderSummaryViewModel.Order);
            _unitOfWork.Save();
            await notificationTask("Sold", $"Add Order With Id {soldOrderSummaryViewModel.Order.Id}");
            foreach (var product in _productList)
            {
                var productDetailDb = await _unitOfWork.ProductDetail.GetFirstOrDefaultAsync(
                    p => p.BranchID == soldOrderSummaryViewModel.BranchId &&
                         p.ProductID == product.Product.Id);
                productDetailDb.Quantity = productDetailDb.Quantity - product.Count;
                await _unitOfWork.ProductDetail.Update(productDetailDb);
                OrderDetail orderDetail = new OrderDetail()
                {
                    Price = product.Product.Price,
                    Quantity = product.Count,
                    ProductDetailId = productDetailDb.Id,
                    OrderID = soldOrderSummaryViewModel.Order.Id
                };
                soldOrderSummaryViewModel.Order.Amount +=
                    product.Count * product.Product.Price * (1-soldOrderSummaryViewModel.Discount); 
                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                await notificationTask("Sold", $"Add OrderDetail With Id {orderDetail}");
            }

            var debt = soldOrderSummaryViewModel.Order.Amount - soldOrderSummaryViewModel.PaidAmount;
            soldOrderSummaryViewModel.Order.Debt = debt;
            await _unitOfWork.Order.Update(soldOrderSummaryViewModel.Order);
            Account account = new Account()
            {
                TransactDate = DateTime.Today,
                Debt = debt,
                Credit = soldOrderSummaryViewModel.PaidAmount,
                CustomerId = soldOrderSummaryViewModel.CustomerId,
                OrderId = soldOrderSummaryViewModel.Order.Id
            };
            await _unitOfWork.Account.AddAsync(account);
            _productList.Clear();
            _customerID = 0;
            _unitOfWork.Save();
            await notificationTask("Sold", $"Update order with id {soldOrderSummaryViewModel.Order.Id} and Add account with id {account.Id}");
            return RedirectToAction(nameof(OrderConfirmation));
        }
        public IActionResult Plus(int cartId)
        {
            var product = _productList.Find(p => p.Product.Id == cartId);
            product.Count += 1;
            return RedirectToAction(nameof(Product));
        }
        
        public IActionResult Minus(int cartId)
        {
            var product = _productList.Find(p => p.Product.Id == cartId);
            if (product.Count == 1)
            {
                _productList.Remove(product);
            }
            else
            {
                product.Count -= 1;
            }
            return RedirectToAction(nameof(Product));
        }
        
        public IActionResult Remove(int cartId)
        {
            var product = _productList.Find(p => p.Product.Id == cartId);
            _productList.Remove(product);
            return RedirectToAction(nameof(Product));
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