using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allCategoryServices = await _unitOfWork.CategoryService.GetAllAsync();
            return Json(new { data = allCategoryServices });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getAllService = await _unitOfWork.CategoryService.GetAsync(id);
            if (getAllService == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.CategoryService.RemoveAsync(getAllService);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}