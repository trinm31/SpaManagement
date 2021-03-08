using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IOrderRepository: IRepositoryAsync<Order>
    {
        Task Update(Order urder);
    }
}