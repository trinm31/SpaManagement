using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allProduct = await _unitOfWork.ProductDetail.GetAllAsync(includeProperties: "Branch,Product");
            return Json(new { data = allProduct });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getProductDetail = await _unitOfWork.ProductDetail.GetAsync(id);
            if (getProductDetail == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.ProductDetail.RemoveAsync(getProductDetail);
            await notificationTask("ProductDetails",$"{getProductDetail.BranchID} and {getProductDetail.ProductID}");
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
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