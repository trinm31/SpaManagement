using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class BranchRepository: RepositoryAsync<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _db;
        public BranchRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Branch branch)
        {
            _db.Update(branch);
        }
    }
}