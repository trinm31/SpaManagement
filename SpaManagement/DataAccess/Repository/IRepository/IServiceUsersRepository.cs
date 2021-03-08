using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IServiceUsersRepository: IRepositoryAsync<ServiceUsers>
    {
        Task Update(ServiceUsers serviceUsers);
    }
}