using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class StaffUserRepository: RepositoryAsync<StaffUser>, IStaffUserRepository
    {
        private readonly ApplicationDbContext _db;
        public StaffUserRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task Update(StaffUser staffUser)
        {
            _db.Update(staffUser);
        }
    }
}