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
    public class AccountsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allAccount = await _unitOfWork.Account.GetAllAsync(includeProperties: "Customer");
            return Json(new { data = allAccount });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var getAccount = await _unitOfWork.Account.GetAsync(id);
            if (getAccount == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Account.RemoveAsync(getAccount);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}