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
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allOrders = await _unitOfWork.Order.GetAllAsync(includeProperties: "Customer");
            return Json(new { data = allOrders });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getOrder = await _unitOfWork.Order.GetAsync(id);
            if (getOrder == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Order.RemoveAsync(getOrder);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}