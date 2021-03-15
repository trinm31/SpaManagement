using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    public class SoldController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SoldController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}