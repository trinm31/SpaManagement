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
            Account = new AccountRepository(_db);
            CategoryService = new CategoryServiceRepository(_db);
            Manufacturer = new ManufacturerRepository(_db);
            Order = new OrderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Product = new ProductRepository(_db);
            ProductDetail = new ProductDetailRepository(_db);
            ServiceDetail = new ServiceDetailRepository(_db);
            ServiceUsers = new ServiceUsersRepository(_db);
            Supplier = new SupplierRepository(_db);
            TypeOfProduct = new TypeOfProductRepository(_db);
            Notification = new NotificationRepository(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IStaffUserRepository Staff { get;  private set;}
        public IBranchRepository Branch { get; private set;}
        public IAccountRepository Account { get; private set;}
        public ICategoryServiceRepository CategoryService { get; private set;}
        public IManufacturerRepository Manufacturer { get; private set;}
        public INotificationRepository Notification { get; private set;}
        public IOrderRepository Order { get; private set;}
        public IOrderDetailRepository OrderDetail { get; private set;}
        public IProductRepository Product { get; private set;}
        public IProductDetailRepository ProductDetail { get; private set;}
        public IServiceDetailRepository ServiceDetail { get; private set;}
        public IServiceUsersRepository ServiceUsers { get; private set;}
        public ISupplierRepository Supplier { get; private set;}
        public ITypeOfProductRepository TypeOfProduct { get; private set;}


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}