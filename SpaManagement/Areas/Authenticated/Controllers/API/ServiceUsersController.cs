using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ServiceUsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceUsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allPServiceUsers = await _unitOfWork.ServiceUsers.GetAllAsync(includeProperties: "ApplicationUser,ServiceDetail");
            return Json(new { data = allPServiceUsers });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getServiceUser = await _unitOfWork.ServiceUsers.GetAsync(id);
            if (getServiceUser == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.ServiceUsers.RemoveAsync(getServiceUser);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}