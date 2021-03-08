using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class SupplierRepository: RepositoryAsync<Supplier>, ISupplierRepository
    {
        private readonly ApplicationDbContext _db;
        public SupplierRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Supplier supplier)
        {
            _db.Update(supplier);
        }
    }
}