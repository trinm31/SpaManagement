using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;

namespace SpaManagement.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allcustomer = await _unitOfWork.Customer.GetAllAsync();
            return Json(new { data = allcustomer });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getcustomer = await _unitOfWork.Customer.GetAsync(id);
            if (getcustomer == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Customer.RemoveAsync(getcustomer);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}