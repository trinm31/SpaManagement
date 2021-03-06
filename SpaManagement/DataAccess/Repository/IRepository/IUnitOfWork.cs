using System;

namespace SpaManagement.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        void Save();
    }
}