using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class TypeOfProductRepository: RepositoryAsync<TypeOfProduct>, ITypeOfProductRepository
    {
        private readonly ApplicationDbContext _db;
        public TypeOfProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(TypeOfProduct typeOfProduct)
        {
            _db.Update(typeOfProduct);
        }
    }
}