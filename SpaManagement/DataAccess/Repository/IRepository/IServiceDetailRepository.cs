using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IServiceDetailRepository: IRepositoryAsync<ServiceDetail>
    {
        Task Update(ServiceDetail serviceDetail);
    }
}