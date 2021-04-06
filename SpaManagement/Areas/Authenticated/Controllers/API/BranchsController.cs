using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BranchsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allbranch = await _unitOfWork.Branch.GetAllAsync();
            return Json(new { data = allbranch });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getBranch = await _unitOfWork.Branch.GetAsync(id);
            if (getBranch == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Branch.RemoveAsync(getBranch);
            _unitOfWork.Save();
            await notificationTask("Branch", $"{getBranch.Name}");
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