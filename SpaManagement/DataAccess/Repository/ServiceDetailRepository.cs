using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class ServiceDetailRepository: RepositoryAsync<ServiceDetail>, IServiceDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public ServiceDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(ServiceDetail serviceDetail)
        {
            _db.Update(serviceDetail);
        }
    }
}