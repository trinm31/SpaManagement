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
    public class ServiceDetailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allPServiceDetails = await _unitOfWork.ServiceDetail.GetAllAsync(includeProperties: "CategoryService,Customer");
            return Json(new { data = allPServiceDetails });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getServiceDetail = await _unitOfWork.ServiceDetail.GetAsync(id);
            if (getServiceDetail == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.ServiceDetail.RemoveAsync(getServiceDetail);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}