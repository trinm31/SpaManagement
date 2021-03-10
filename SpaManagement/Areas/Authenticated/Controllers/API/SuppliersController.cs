using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SuppliersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuppliersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allsuppliers = await _unitOfWork.Supplier.GetAllAsync();
            return Json(new { data = allsuppliers });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getSupplier = await _unitOfWork.Supplier.GetAsync(id);
            if (getSupplier == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Supplier.RemoveAsync(getSupplier);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}