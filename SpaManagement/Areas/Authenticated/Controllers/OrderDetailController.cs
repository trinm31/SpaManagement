using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class OrderDetailController: Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public OrderDetailViewModel OrderDetailViewModel { get; set; }
        public OrderDetailController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var orderDetails = _db.OrderDetails.
                Where(o => o.OrderID == id).Include(o => o.ProductDetail)
                .ThenInclude(o => o.Product);
            var orders = await _unitOfWork.Order.GetFirstOrDefaultAsync(o=> o.Id == id, includeProperties:"Customer");
            OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel()
            {
                OrderDetails = orderDetails,
                Order = orders
            };
            return View(orderDetailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo()
        {
            var orderFrDB = await _unitOfWork.Order.GetFirstOrDefaultAsync(o=> o.Id == OrderDetailViewModel.Order.Id);
            orderFrDB.OrderType = OrderDetailViewModel.Order.OrderType;
            orderFrDB.Amount = OrderDetailViewModel.Order.Amount;
            orderFrDB.PaidAmount = OrderDetailViewModel.Order.PaidAmount;
            orderFrDB.Note = OrderDetailViewModel.Order.Note;
            await _unitOfWork.Order.Update(orderFrDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region API

        [HttpGet]
        public async Task<IActionResult> getOrderList()
        {
            var orderList = await _unitOfWork.Order.GetAllAsync(includeProperties: "Customer");
            return Json(new {data = orderList});
        }
        

        #endregion
    }
}