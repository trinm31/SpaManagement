using SpaManagement.DataAccess.Data;
using SpaManagement.DataAccess.Repository.IRepository;

namespace SpaManagement.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Customer = new CustomerRepository(_db);
            Staff = new StaffUserRepository(_db);
            Branch = new BranchRepository(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IStaffUserRepository Staff { get;  private set;}
        public IBranchRepository Branch { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}