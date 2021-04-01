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
using SpaManagement.Utility.Enum;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class ReturnController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerID;
        public ReturnController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult ReturnConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> Return(int id)
        {
            if (id != 0)
            {
                _customerID = id;
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var currentUser = await _unitOfWork.Staff.GetAsync(claims.Value);
            var branchId = currentUser.BranchId;
            IEnumerable<Product> productList = await _unitOfWork.Product.GetAllAsync();
            ReturnViewModels returnViewModels = new ReturnViewModels()
            {
                CustomerId = id,
                BranchId = branchId.Value,
                ProductList = productList.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Order = new Order(),
            };
            returnViewModels.Order.OrderDate = DateTime.Today;
            returnViewModels.Order.OrderType = OrderType.Returns;
            return View(returnViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnViewModels returnViewModels)
        {
            var product = await _unitOfWork.Product.GetAsync(returnViewModels.ProductID);
            if (!(product.Price * returnViewModels.Quantity == returnViewModels.PaidAmount))
            {
                // tra loi
            }

            var productDetailDb = await _unitOfWork.ProductDetail.GetFirstOrDefaultAsync(p =>
                p.BranchID == returnViewModels.BranchId
                && p.ProductID == returnViewModels.ProductID);
            productDetailDb.Quantity += returnViewModels.Quantity;
            await _unitOfWork.ProductDetail.Update(productDetailDb);
            returnViewModels.Order.PaidAmount = returnViewModels.PaidAmount;
            returnViewModels.Order.Note = returnViewModels.Note;
            returnViewModels.Order.CustomerId = returnViewModels.CustomerId;
            returnViewModels.Order.Amount = returnViewModels.PaidAmount * returnViewModels.Quantity;
            await _unitOfWork.Order.AddAsync(returnViewModels.Order);
            _unitOfWork.Save();
            OrderDetail orderDetail = new OrderDetail()
            {
                Price = product.Price,
                Quantity = returnViewModels.Quantity,
                ProductDetailId = productDetailDb.Id,
                OrderID = returnViewModels.Order.Id,
            };
            await _unitOfWork.OrderDetail.AddAsync(orderDetail);
            Account account = new Account()
            {
                TransactDate = DateTime.Today,
                Credit = returnViewModels.PaidAmount,
                CustomerId = returnViewModels.CustomerId,
                OrderId = returnViewModels.Order.Id
            };
            await _unitOfWork.Account.AddAsync(account);
            _unitOfWork.Save();
            return RedirectToAction(nameof(ReturnConfirmation));
        }
    }
}