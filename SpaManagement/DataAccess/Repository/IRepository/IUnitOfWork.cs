using System;
using SpaManagement.Models;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICustomerRepository Customer { get; }
        IStaffUserRepository Staff { get; }
        IBranchRepository Branch { get; }
        IAccountRepository Account { get; }
        ICategoryServiceRepository CategoryService { get; }
        IManufacturerRepository Manufacturer { get; }
        INotificationRepository Notification { get; }
        IOrderRepository Order { get; }
        IOrderDetailRepository OrderDetail { get;}
        IProductRepository Product { get; }
        IProductDetailRepository ProductDetail { get; }
        IServiceDetailRepository ServiceDetail { get;}
        IServiceUsersRepository ServiceUsers { get; }
        ISupplierRepository Supplier { get;}
        ITypeOfProductRepository TypeOfProduct { get; }
        
        void Save();
    }
}