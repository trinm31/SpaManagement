using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerId;
        private static List<ServiceCategoryDetailViewModel> _serviceList;
        public ServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Service(int id)
        {
            if (id != 0)
            {
                _customerId = id;
            }
            return View();
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }
        public IActionResult Summary()
        {
            return View();
        } 
        
        public IActionResult Plus(int cartId)
        {
            var product = _serviceList.Find(p => p.CategoryService.Id == cartId);
            product.Count += 1;
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Minus(int cartId)
        {
            var product = _serviceList.Find(p => p.CategoryService.Id == cartId);
            if (product.Count == 1)
            {
                _serviceList.Remove(product);
            }
            else
            {
                product.Count -= 1;
            }
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Remove(int cartId)
        {
            var product = _serviceList.Find(p => p.CategoryService.Id == cartId);
            _serviceList.Remove(product);
            return RedirectToAction(nameof(Summary));
        }
    }
}