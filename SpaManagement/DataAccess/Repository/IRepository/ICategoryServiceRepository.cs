using System.Threading.Tasks;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface ICategoryServiceRepository: IRepositoryAsync<CategoryService>
    {
        Task Update(CategoryService categoryService);
    }
}