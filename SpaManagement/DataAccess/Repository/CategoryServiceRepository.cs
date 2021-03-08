using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class CategoryServiceRepository: RepositoryAsync<CategoryService>, ICategoryServiceRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(CategoryService categoryService)
        {
            _db.Update(categoryService);
        }
    }
}