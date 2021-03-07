using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface ICustomerRepository: IRepositoryAsync<Customer>
    {
        Task Update(Customer customer);
    }
}