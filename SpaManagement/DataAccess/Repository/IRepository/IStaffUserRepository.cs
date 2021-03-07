using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IStaffUserRepository : IRepositoryAsync<StaffUser>
    {
        Task Update(StaffUser staffUser);
    }
}