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
    public class ProductDetailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductDetailViewModel productDetailViewModel)
        {
            IEnumerable<Branch> branchList = await _unitOfWork.Branch.GetAllAsync();
            IEnumerable<Product> productList = await _unitOfWork.Product.GetAllAsync();
            if (ModelState.IsValid)
            {
                var IsProductDetailsExited =
                    await _unitOfWork.ProductDetail
                        .GetAllAsync(i => i.BranchID == productDetailViewModel.ProductDetails.BranchID
                                          && i.ProductID == productDetailViewModel.ProductDetails.ProductID);
                if (productDetailViewModel.ProductDetails.Id == 0)
                {
                    if (IsProductDetailsExited.Any())
                    {
                        productDetailViewModel = new ProductDetailViewModel()
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
                        return View(productDetailViewModel);
                    }
                    else
                    {
                        await _unitOfWork.ProductDetail.AddAsync(productDetailViewModel.ProductDetails);
                    }
                    
                }
               
                if (productDetailViewModel.ProductDetails.Id != 0) 
                {
                    if (IsProductDetailsExited.Any())
                    {
                        productDetailViewModel = new ProductDetailViewModel()
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
                        ViewData["Message"] = "Error: Name already exists";
                        return View(productDetailViewModel);
                    }
                    else
                    {
                        await _unitOfWork.ProductDetail.Update(productDetailViewModel.ProductDetails);
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            productDetailViewModel = new ProductDetailViewModel()
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
            return View(productDetailViewModel);
        }
    }
}