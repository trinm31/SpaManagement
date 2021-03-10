using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ManufacturersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManufacturersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allmanufacturer = await _unitOfWork.Manufacturer.GetAllAsync();
            return Json(new { data = allmanufacturer });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getManufacturer = await _unitOfWork.Manufacturer.GetAsync(id);
            if (getManufacturer == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Manufacturer.RemoveAsync(getManufacturer);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}