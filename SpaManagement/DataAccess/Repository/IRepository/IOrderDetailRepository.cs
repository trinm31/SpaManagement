using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository: IRepositoryAsync<OrderDetail>
    {
        Task Update(OrderDetail orderDetail);
    }
}