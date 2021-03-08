using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class ProductDetailRepository: RepositoryAsync<ProductDetail>, IProductDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(ProductDetail productDetail)
        {
            _db.Update(productDetail);
        }
    }
}