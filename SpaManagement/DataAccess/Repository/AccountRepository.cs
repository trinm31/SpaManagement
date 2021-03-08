using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class AccountRepository: RepositoryAsync<Account>, IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Account account)
        {
            _db.Update(account);
        }
    }
}