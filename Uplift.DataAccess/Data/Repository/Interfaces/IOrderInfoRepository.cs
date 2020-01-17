using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IOrderInfoRepository : IRepository<OrderInfo>
    {
        void ChangeOrderStatus(int orderInfoId, string status);
    }
}
