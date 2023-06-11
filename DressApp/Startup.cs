using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using DressApp.WebUi.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace DressApp.WebUi
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

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("server=LAPTOP-LM2N83TK;database=DressAppShopp;" +
                            "integrated security=true;"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 7;


                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail=true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

            
            });

            services.ConfigureApplicationCookie(options => {

                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accesdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            
            
            });

            
            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
         
            app.UseStaticFiles();

			app.UseAuthentication();
			app.UseRouting();
			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                  name: "checkout",
                  pattern: "checkout",
                defaults: new { controller = "Cart", action = "Checkout" });

                endpoints.MapControllerRoute(
                  name: "cart",
                  pattern: "cart",
                defaults: new { controller = "Cart", action = "Index" });

                endpoints.MapControllerRoute(
                  name: "search",
                  pattern: "search",
                defaults: new {controller= "User",action="search" });

                endpoints.MapControllerRoute(
                  name: "adminroleedit",
                  pattern: "admin/RoleEdit/{id?}",
                defaults: new { controller = "Admin", action = "RoleEdit" });

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=User}/{action=Page}/{id?}");


               
            });
        }
    }
}
