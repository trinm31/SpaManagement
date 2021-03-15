using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
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
                    importViewModel = new ImportViewModel()
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
                        ProductDetails = new ProductDetail()
                    };
                    ViewData["Message"] = "Error: Quantity for this product in this branch already exists";
                    return View(importViewModel);
                }
                else
                {
                    await _unitOfWork.ProductDetail.AddAsync(importViewModel.ProductDetails);
                    var productDb = await _unitOfWork.Product.GetAsync(importViewModel.ProductDetails.ProductID);
                    productDb.ImportPrice = importViewModel.Price;
                    await _unitOfWork.Product.Update(productDb);
                    var amount = importViewModel.Price * importViewModel.ProductDetails.Quantity;
                    var debt = amount - importViewModel.PaidAmount;
                    importViewModel.Order.PaidAmount = importViewModel.PaidAmount;
                    importViewModel.Order.Note = importViewModel.Note;
                    importViewModel.Order.Amount = amount;
                    importViewModel.Order.Debt = debt;
                    await _unitOfWork.Order.AddAsync(importViewModel.Order);
                    _unitOfWork.Save();
                    
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
                    Account account = new Account()
                    {
                        TransactDate = DateTime.Today,
                        Debt = debt,
                        Credit = importViewModel.PaidAmount,
                    };
                    await _unitOfWork.Account.AddAsync(account);
                    _unitOfWork.Save();
                    ViewData["Message"] = "Success: Create Successfully";
                }
            }
            return View();
        }
    }
}