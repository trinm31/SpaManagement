using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository: IRepositoryAsync<ApplicationUser>
    {
        Task Update(ApplicationUser applicationUser);
    }
}