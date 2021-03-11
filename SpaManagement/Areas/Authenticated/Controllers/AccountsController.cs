using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
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
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            AccountViewModel accountViewModel = new AccountViewModel()
            {
                CustomersList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                Account = new Account()
            };
            if (id == null)
            {
                return View(accountViewModel);
            }

            accountViewModel.Account = await _unitOfWork.Account.GetAsync(id.GetValueOrDefault());
            if (accountViewModel.Account == null)
            {
                return NotFound();
            }
            return View(accountViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(AccountViewModel accountViewModel)
        {
            IEnumerable<Customer> customers = await _unitOfWork.Customer.GetAllAsync();
            if (ModelState.IsValid)
            {
                if (accountViewModel.Account.Id == 0)
                {
                    await _unitOfWork.Account.AddAsync(accountViewModel.Account);
                }
               
                if (accountViewModel.Account.Id  != 0) 
                {
                    await _unitOfWork.Account.Update(accountViewModel.Account);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            accountViewModel = new AccountViewModel()
            {
                CustomersList = customers.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.id.ToString()
                }),
                Account = new Account()
            };
            return View(accountViewModel);
        }
    }
}