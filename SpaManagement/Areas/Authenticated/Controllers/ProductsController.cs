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
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<TypeOfProduct> typeOfProducts = await _unitOfWork.TypeOfProduct.GetAllAsync();
            IEnumerable<Supplier> suppliers = await _unitOfWork.Supplier.GetAllAsync();
            IEnumerable<Manufacturer> manufacturers = await _unitOfWork.Manufacturer.GetAllAsync();
            ProductsViewModel productsViewModel = new ProductsViewModel()
            {
                TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Suppliers = suppliers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Manufacturers = manufacturers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Quantity = 0,
                Product = new Product()
            };
            if (id == null)
            {
                return View(productsViewModel);
            }
            IEnumerable<ProductDetail> productDetails = await _unitOfWork.ProductDetail.GetAllAsync(p=> p.ProductID == id);
            foreach (var productDetail in productDetails)
            {
                productsViewModel.Quantity += productDetail.Quantity;
            }
            productsViewModel.Product = await _unitOfWork.Product.GetAsync(id.GetValueOrDefault());
            if (productsViewModel.Product == null)
            {
                return NotFound();
            }
            return View(productsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductsViewModel productsViewModel)
        {
            IEnumerable<TypeOfProduct> typeOfProducts = await _unitOfWork.TypeOfProduct.GetAllAsync();
            IEnumerable<Supplier> suppliers = await _unitOfWork.Supplier.GetAllAsync();
            IEnumerable<Manufacturer> manufacturers = await _unitOfWork.Manufacturer.GetAllAsync();
            IEnumerable<ProductDetail> productDetails = await _unitOfWork.ProductDetail.GetAllAsync(p=> p.ProductID == productsViewModel.Product.Id);
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.Product
                        .GetAllAsync(c => c.Name == productsViewModel.Product.Name && c.Id != productsViewModel.Product.Id);
                var BarcodeFromDb =
                    await _unitOfWork.Product
                        .GetAllAsync(c => c.BarcodeId == productsViewModel.Product.BarcodeId && c.Id != productsViewModel.Product.Id);
                if (productsViewModel.Product.Id == 0)
                {
                    if (nameFromDb.Any())
                    {
                        productsViewModel = new ProductsViewModel()
                        {
                            TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Suppliers = suppliers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Manufacturers = manufacturers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Quantity = 0,
                            Product = new Product()
                        };
                        foreach (var productDetail in productDetails)
                        {
                            productsViewModel.Quantity += productDetail.Quantity;
                        }
                        ViewData["Message"] = "Error: Name already exists";
                        return View(productsViewModel);
                    } 
                    else if (BarcodeFromDb.Any())
                    {
                        productsViewModel = new ProductsViewModel()
                        {
                            TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Suppliers = suppliers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Manufacturers = manufacturers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Quantity = 0,
                            Product = new Product()
                        };
                        foreach (var productDetail in productDetails)
                        {
                            productsViewModel.Quantity += productDetail.Quantity;
                        }
                        ViewData["Message"] = "Error: Barcode Id already exists";
                        return View(productsViewModel);
                    }
                    else
                    {
                        await _unitOfWork.Product.AddAsync(productsViewModel.Product);
                        await notificationTask("Product",$"Add {productsViewModel.Product.Id}");
                    }
                    
                }
               
                if (productsViewModel.Product.Id != 0) 
                {
                    if (nameFromDb.Any())
                    {
                        productsViewModel = new ProductsViewModel()
                        {
                            TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Suppliers = suppliers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Manufacturers = manufacturers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Quantity = 0,
                            Product = new Product()
                        };
                        ViewData["Message"] = "Error: Name already exists";
                        return View(productsViewModel);
                    } 
                    else if (BarcodeFromDb.Any())
                    {
                        productsViewModel = new ProductsViewModel()
                        {
                            TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Suppliers = suppliers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Manufacturers = manufacturers.Select(I => new SelectListItem
                            {
                                Text = I.Name,
                                Value = I.Id.ToString()
                            }),
                            Quantity = 0,
                            Product = new Product()
                        };
                        foreach (var productDetail in productDetails)
                        {
                            productsViewModel.Quantity += productDetail.Quantity;
                        }
                        ViewData["Message"] = "Error: Branch Id already exists";
                        return View(productsViewModel);
                    }
                    else
                    {
                        await _unitOfWork.Product.Update(productsViewModel.Product);
                        await notificationTask("Product",$"Update {productsViewModel.Product.Id}");
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            productsViewModel = new ProductsViewModel()
            {
                TypeOfProducts = typeOfProducts.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Suppliers = suppliers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Manufacturers = manufacturers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                }),
                Quantity = 0,
                Product = new Product()
            };
            foreach (var productDetail in productDetails)
            {
                productsViewModel.Quantity += productDetail.Quantity;
            }
            return View(productsViewModel);
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