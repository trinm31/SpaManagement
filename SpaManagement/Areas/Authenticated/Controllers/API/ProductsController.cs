using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allProduct = await _unitOfWork.Product.GetAllAsync(includeProperties: "TypeOfProduct,Supplier,Manufacturer");
            return Json(new { data = allProduct });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getproduct = await _unitOfWork.Product.GetAsync(id);
            if (getproduct == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Product.RemoveAsync(getproduct);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}