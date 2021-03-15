using System.Collections.Generic;
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

        public SoldController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Product));
            }

            SoldDetailViewModel soldDetailViewModel = new SoldDetailViewModel()
            {
                Product = product
            };
            return View(soldDetailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(SoldDetailViewModel soldDetailViewModel)
        {
            /*var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var currentUser = await _unitOfWork.Staff.GetAsync(claims.Value);
            var branchId = currentUser.BranchId;*/
            var ProductExits = _productList.FindAll(p =>p.Product.Id == soldDetailViewModel.Product.Id);
            if (ProductExits.Any())
            {
                var product = _productList.Find(p => p.Product.Id == soldDetailViewModel.Product.Id);
                product.Count += soldDetailViewModel.Count;
                _productList.Remove(product);
                _productList.Add(product);
                //theem message
            }
            else
            {
                _productList.Add(soldDetailViewModel);
                //them message
            }
            return RedirectToAction(nameof(Product));
        }

        public async Task<IActionResult> Summary()
        {
            return View();
        }
    }
}