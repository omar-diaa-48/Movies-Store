using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo
{
    public class Startup
    {
        //Hello
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<MovieStoreDBContext>(

                options => options.UseSqlServer(Configuration.GetConnectionString("ConnString")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireNonAlphanumeric = false;
            })
                     .AddEntityFrameworkStores<MovieStoreDBContext>()
                     .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "BlockBusterCookie";
                config.LoginPath = "/User/Login";
            });

            services.AddScoped<Order>(sp => Order.Getorder(sp));

            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "272076720964060";
                options.AppSecret = "0ebf82b45f11b1f530e751be8a98626a";
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            });

            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");

                options.ClientId = "161841411931-l230qvs0aclrnpon9h9odgf5cagsu6eu.apps.googleusercontent.com";
                options.ClientSecret = "-oi2U3KX0z2IACJLc9miZzNT";
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddRazorPages();

            services.AddSingleton<IEmailSender, EmailSender>();

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
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                   name: "search",
                   pattern: "{controller=Movies}/{action=Search}/{condition}/{search}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

        }
    }
}
