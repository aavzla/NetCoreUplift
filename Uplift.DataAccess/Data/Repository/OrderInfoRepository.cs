using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderInfoRepository : Repository<OrderInfo>, IOrderInfoRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderInfoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void ChangeOrderStatus(int orderInfoId, string status)
        {
            var orderFromDB = GetFirstOrDefault(o => o.Id == orderInfoId);
            orderFromDB.Status = status;
            _db.SaveChanges();
        }
    }
}
