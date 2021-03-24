using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface ISlotRepository: IRepositoryAsync<Slot>
    {
        Task Update(Slot slot);
    }
}