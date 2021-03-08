using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface ITypeOfProductRepository: IRepositoryAsync<TypeOfProduct>
    {
        Task Update(TypeOfProduct typeOfProduct);
    }
}