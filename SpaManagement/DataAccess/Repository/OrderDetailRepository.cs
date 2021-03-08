using System.Threading.Tasks;
using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository
{
    public class OrderDetailRepository: RepositoryAsync<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }
    }
}