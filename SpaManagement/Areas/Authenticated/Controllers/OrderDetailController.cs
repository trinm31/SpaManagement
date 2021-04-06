using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin)]
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
            var accountDb = await _unitOfWork.Account.GetFirstOrDefaultAsync(a =>
                a.CustomerId == orderFrDB.CustomerId &&
                a.OrderId == orderFrDB.Id);
            accountDb.Credit = orderFrDB.PaidAmount;
            accountDb.Debt = Math.Abs(orderFrDB.Amount - orderFrDB.PaidAmount);
            await _unitOfWork.Account.Update(accountDb);
            _unitOfWork.Save();
            await notificationTask("OrderDetail", $"Update {orderFrDB.Id}");
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
        [NonAction]
        private async Task notificationTask(string controller, string action = null)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userDb = await _unitOfWork.ApplicationUser.GetAsync(claims.Value);
            string Notimessage = $"User {userDb.Name} delete {controller} for {action}";
            Notification notification = new Notification()
            {
                Date = DateTime.Today,
                Content = Notimessage
            };
            await _unitOfWork.Notification.AddAsync(notification);
            _unitOfWork.Save();
        }
    }
}