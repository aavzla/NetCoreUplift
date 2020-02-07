using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Data.Initializer
{
    public class DBInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                //Need some log.
                //throw;
            }

            if (_db.Roles.Any(r => r.Name == StaticDetails.Admin))
            {
                return;
            }

            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Manager)).GetAwaiter().GetResult();

            string adminEmail = "admin@gmail.com";

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Name = "Admin"
            }, "Admin123!").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.First(u => u.Email == adminEmail);
            _userManager.AddToRoleAsync(user, StaticDetails.Admin).GetAwaiter().GetResult();
        }
    }
}
