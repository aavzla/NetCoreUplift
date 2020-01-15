using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
            var objFromDB = GetFirstOrDefault(x => x.Id == service.Id);

            objFromDB.Name = service.Name;
            objFromDB.Description = service.Description;
            objFromDB.Price = service.Price;
            objFromDB.ImageUrl = service.ImageUrl;
            objFromDB.FrequencyId = service.FrequencyId;
            objFromDB.CategoryId = service.CategoryId;

            _db.SaveChanges();
        }
    }
}
