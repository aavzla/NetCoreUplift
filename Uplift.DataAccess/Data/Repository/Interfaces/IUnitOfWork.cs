using System;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IFrequencyRepository Frequencies { get; }
        IServiceRepository Services { get; }
        IOrderInfoRepository OrderInfos { get; }
        IOrderDetailsRepository OrderDetails { get; }
        int Save();
    }
}
