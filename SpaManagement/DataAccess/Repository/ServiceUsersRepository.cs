using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class ServiceUsersRepository: RepositoryAsync<ServiceUsers>, IServiceUsersRepository
    {
        private readonly ApplicationDbContext _db;
        public ServiceUsersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(ServiceUsers serviceUsers)
        {
            _db.Update(serviceUsers);
        }
    }
}