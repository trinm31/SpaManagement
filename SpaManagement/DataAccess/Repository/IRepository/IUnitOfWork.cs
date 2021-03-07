using System;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICustomerRepository Customer { get; }
        IStaffUserRepository Staff { get; }
        IBranchRepository Branch { get; }
        void Save();
    }
}