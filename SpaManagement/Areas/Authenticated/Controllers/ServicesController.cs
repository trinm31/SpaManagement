using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;
using SpaManagement.Utility;
using SpaManagement.ViewModels;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class ServicesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _customerId;
        private static List<ServiceCategoryDetailViewModel> _serviceList;
        public ServicesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET
        
        
        
    }
}