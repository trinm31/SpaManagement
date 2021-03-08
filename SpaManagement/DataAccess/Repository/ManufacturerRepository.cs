using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class ManufacturerRepository: RepositoryAsync<Manufacturer>, IManufacturerRepository
    {
        private readonly ApplicationDbContext _db;
        public ManufacturerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Manufacturer manufacturer)
        {
            _db.Update(manufacturer);
        }
    }
}