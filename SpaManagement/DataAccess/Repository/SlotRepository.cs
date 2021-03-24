using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class SlotRepository: RepositoryAsync<Slot>, ISlotRepository
    {
        private readonly ApplicationDbContext _db;
        public SlotRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Slot slot)
        {
            _db.Update(slot);
        }
    }
}