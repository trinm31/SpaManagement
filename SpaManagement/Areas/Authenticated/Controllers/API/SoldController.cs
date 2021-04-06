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
        private readonly IUnitOfWork _unitOfWork;
        public SoldController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> GetCustomer()
        {
            var customerList = await _unitOfWork.Customer.GetAllAsync(c => c.id != 1);
            return Json(new { data = customerList });
        }
        public async Task<IActionResult> GetProduct()
        {
            var productList = await _unitOfWork.Product.GetAllAsync();
            return Json(new { data = productList });
        }
        
    }
}