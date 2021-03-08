using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface ISupplierRepository: IRepositoryAsync<Supplier>
    {
        Task Update(Supplier supplier);
    }
}