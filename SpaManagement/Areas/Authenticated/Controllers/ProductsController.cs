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
                Product = new Product()
            };
            if (id == null)
            {
                return View(productsViewModel);
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
                            Product = new Product()
                        };
                        ViewData["Message"] = "Error: Barcode Id already exists";
                        return View(productsViewModel);
                    }
                    else
                    {
                        await _unitOfWork.Product.AddAsync(productsViewModel.Product);
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
                            Product = new Product()
                        };
                        ViewData["Message"] = "Error: Branch Id already exists";
                        return View(productsViewModel);
                    }
                    else
                    {
                        await _unitOfWork.Product.Update(productsViewModel.Product);
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
                Product = new Product()
            };
            return View(productsViewModel);
        }
    }
}