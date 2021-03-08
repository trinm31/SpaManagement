using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IAccountRepository: IRepositoryAsync<Account>
    {
        Task Update(Account account);
    }
}