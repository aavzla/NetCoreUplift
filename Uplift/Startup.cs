using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Uplift.DataAccess.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Initializer;

namespace Uplift
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //By Changing the AddDefaultIdentity for AddIdentity allow us to add Users with role and the default option doesn't allow to do that.
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //The below lines were already included in the AddDefaultIdentity method, but we change that, so why we need to include them.
                //If we want to use two factor authentification
                //or the token system for send a token when the user forget a password, we add the line below.
                .AddDefaultTokenProviders()
                //Adds a default, self-contained UI for Identity to the application using Razor Pages in an area named Identity.
                .AddDefaultUI();

            //We add AddNewtonsoftJson for handling the JSON objects when we are calling to the API. 
            services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            //Configure the service Session to handle the shopping cart in the project.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //Add Unit of work (UoW) to the container for injection purposes.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add DBInitializer to the service
            services.AddScoped<IDbInitializer, DBInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //Configure the session
            app.UseSession();
            app.UseRouting();
            //Configure DBInitializer
            dbInitializer.Initialize();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
