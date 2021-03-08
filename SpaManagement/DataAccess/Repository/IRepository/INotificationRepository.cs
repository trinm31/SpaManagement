using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface INotificationRepository: IRepositoryAsync<Notification>
    {
        Task Update(Notification notification);
    }
}