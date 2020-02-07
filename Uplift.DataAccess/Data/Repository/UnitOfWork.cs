using Uplift.DataAccess.Data.Repository.Interfaces;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository Categories { get; private set; }
        public IFrequencyRepository Frequencies { get; private set; }
        public IServiceRepository Services { get; private set; }
        public IOrderInfoRepository OrderInfos { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IUserRepository Users { get; private set; }
        public ISP_Call SP_Call { get; private set; }
        public IWebImageRepository WebImages { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new CategoryRepository(_db);
            Frequencies = new FrequencyRepository(_db);
            Services = new ServiceRepository(_db);
            OrderInfos = new OrderInfoRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            Users = new UserRepository(_db);
            SP_Call = new SP_Call(_db);
            WebImages = new WebImageRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}
