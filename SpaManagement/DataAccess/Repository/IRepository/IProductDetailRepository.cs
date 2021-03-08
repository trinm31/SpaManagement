using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IProductDetailRepository: IRepositoryAsync<ProductDetail>
    {
        Task Update(ProductDetail productDetail);
    }
}