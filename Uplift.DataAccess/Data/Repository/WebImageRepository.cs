using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class WebImageRepository : Repository<WebImage>, IWebImageRepository
    {
        private readonly ApplicationDbContext _db;

        public WebImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
