using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.ReportModel;
using SpaManagement.Utility.Enum;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    public class ImportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Import(int? id)
        {
            IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
            IEnumerable<Product> productList = await _unitOfWork.Product.GetAllAsync();
            ImportViewModel importViewModel = new ImportViewModel()
            {
                BranchList = branchList.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                ProductList = productList.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                ProductDetails = new ProductDetail(),
                Order = new Order(),
            };
            importViewModel.Order.OrderDate = DateTime.Today;
            importViewModel.Order.OrderType = OrderType.Import;
            return View(importViewModel);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(ImportViewModel importViewModel)
        {
            ImportInvoice importInvoice = new ImportInvoice();
            IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
            IEnumerable<Product> productList = await _unitOfWork.Product.GetAllAsync();
            var IsProductDetailsExited =
                await _unitOfWork.ProductDetail
                    .GetAllAsync(i => i.BranchID == importViewModel.ProductDetails.BranchID
                                      && i.ProductID == importViewModel.ProductDetails.ProductID);
            if (importViewModel.ProductDetails.Id == 0)
            {
                if (IsProductDetailsExited.Any())
                {
                    var productDetailDb =
                    await _unitOfWork.ProductDetail
                    .GetFirstOrDefaultAsync(i => i.BranchID == importViewModel.ProductDetails.BranchID
                                      && i.ProductID == importViewModel.ProductDetails.ProductID);
                    productDetailDb.Quantity += importViewModel.ProductDetails.Quantity;
                    await _unitOfWork.ProductDetail.Update(productDetailDb);
                    _unitOfWork.Save();
                    var productDb = await _unitOfWork.Product.GetAsync(importViewModel.ProductDetails.ProductID);
                    var amount = importViewModel.Price * importViewModel.ProductDetails.Quantity;
                    var debt = Math.Abs(amount - importViewModel.PaidAmount);
                    importViewModel.Order.CustomerId = 1;
                    importViewModel.Order.PaidAmount = importViewModel.PaidAmount;
                    importViewModel.Order.Note = importViewModel.Note;
                    importViewModel.Order.Amount = amount;
                    importViewModel.Order.Debt = debt;
                    await _unitOfWork.Order.AddAsync(importViewModel.Order);
                    _unitOfWork.Save();
                    await notificationTask("Import", $"Add Order {importViewModel.Order.Id}");
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Price = importViewModel.Price,
                        Quantity = importViewModel.ProductDetails.Quantity,
                        ProductDetailId = productDetailDb.Id,
                        OrderID = importViewModel.Order.Id
                    };
                    await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                    await notificationTask("Import", $"Add {orderDetail.Id}");
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Debt = debt,
                        Credit = importViewModel.PaidAmount,
                        OrderId = importViewModel.Order.Id,
                        CustomerId = 1
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    _unitOfWork.Save();
                    await notificationTask("Import", $"Add OrderDetail with id {orderDetail.Id}, Account with id {account.Id}");
                    ViewData["Message"] = "Success: Create Successfully";
                    importInvoice.Name = productDb.Name;
                    importInvoice.Price = productDb.Price;
                    importInvoice.Debt = debt;
                    importInvoice.ImportPrice = importViewModel.Price;
                    importInvoice.Quantity = importViewModel.ProductDetails.Quantity;
                }
                else
                {
                    await _unitOfWork.ProductDetail.AddAsync(importViewModel.ProductDetails);
                    var productDb = await _unitOfWork.Product.GetAsync(importViewModel.ProductDetails.ProductID);
                    productDb.ImportPrice = importViewModel.Price;
                    await _unitOfWork.Product.Update(productDb);
                    var amount = importViewModel.Price * importViewModel.ProductDetails.Quantity;
                    var debt = Math.Abs(amount - importViewModel.PaidAmount);
                    importViewModel.Order.CustomerId = 1;
                    importViewModel.Order.PaidAmount = importViewModel.PaidAmount;
                    importViewModel.Order.Note = importViewModel.Note;
                    importViewModel.Order.Amount = amount;
                    importViewModel.Order.Debt = debt;
                    await _unitOfWork.Order.AddAsync(importViewModel.Order);
                    _unitOfWork.Save();
                    await notificationTask("Import",$"Add Order {importViewModel.Order.Id}");
                    var productDetailDb = await _unitOfWork.ProductDetail.GetFirstOrDefaultAsync(p =>
                        p.BranchID == importViewModel.ProductDetails.BranchID
                        && p.ProductID == importViewModel.ProductDetails.ProductID);
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Price = importViewModel.Price,
                        Quantity = importViewModel.ProductDetails.Quantity,
                        ProductDetailId = productDetailDb.Id,
                        OrderID = importViewModel.Order.Id
                    };
                    await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                    await notificationTask("Import",$"Add {orderDetail.Id}");
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Debt = debt,
                        Credit = importViewModel.PaidAmount,
                        OrderId = importViewModel.Order.Id,
                        CustomerId = 1
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    _unitOfWork.Save();
                    await notificationTask("Import",$"Add OrderDetail with id {orderDetail.Id}, Account with id {account.Id}");
                    ViewData["Message"] = "Success: Create Successfully";
                    importInvoice.Name = productDb.Name;
                    importInvoice.Price = productDb.Price;
                    importInvoice.Debt = debt;
                    importInvoice.ImportPrice = importViewModel.Price;
                    importInvoice.Quantity = importViewModel.ProductDetails.Quantity;
                }
            }
            return RedirectToAction("Invoice",importInvoice);
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
        [HttpGet]
        public async Task<IActionResult> Invoice(ImportInvoice importInvoice)
        {
            return new ViewAsPdf("Invoice", importInvoice);
        }
    }
}