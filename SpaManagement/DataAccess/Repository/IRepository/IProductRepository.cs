using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IProductRepository: IRepositoryAsync<Product>
    {
        Task Update(Product product);
    }
}