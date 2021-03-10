using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TypeOfProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeOfProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alltype = await _unitOfWork.TypeOfProduct.GetAllAsync();
            return Json(new { data = alltype });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getType = await _unitOfWork.TypeOfProduct.GetAsync(id);
            if (getType == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.TypeOfProduct.RemoveAsync(getType);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}