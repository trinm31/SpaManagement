using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class SoldController : Controller
    {
        private static List<Product> _productList;
        private static string _studentID;
        private readonly IUnitOfWork _unitOfWork;

        public SoldController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _unitOfWork.Customer.GetAllAsync();
            return Json(new { data = customerList });
        }
        public async Task<IActionResult> GetProduct()
        {
            var productList = await _unitOfWork.Product.GetAllAsync();
            return Json(new { data = productList });
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Add To Cart error" });
            }

            var IsProductExit = _productList.FindAll(p => p == product);
            if (IsProductExit.Any())
            {
                return Json(new { success = false, message = "Product Allready Add" });
            }
           _productList.Add(product);
            return Json(new { success = true, message = "Add To Cart successful" });
        }
    }
}