using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void LockUser(string userId);

        void UnlockUser(string userId);
    }
}
