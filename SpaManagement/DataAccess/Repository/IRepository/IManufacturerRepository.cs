using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IManufacturerRepository: IRepositoryAsync<Manufacturer>
    {
        Task Update(Manufacturer manufacturer);
    }
}