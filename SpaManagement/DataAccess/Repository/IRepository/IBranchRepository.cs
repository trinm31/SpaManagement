using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IBranchRepository : IRepositoryAsync<Branch>
    {
        Task Update(Branch branch);
    }
}