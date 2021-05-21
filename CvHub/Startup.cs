using CVHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CvHub
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
            // Denna service g�r s� att man kan kr�va inloggning p� vissa delar av hemsidan genom att kalla p� [Authorize].
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                    .AddCookie(options =>
                    {
                        //Anv�ndaren skickas hit om �tkomsten nekas.
                        options.LoginPath = "/Validation/SignIn";
                    })
                    .AddFacebook(options =>
                    {
                        //De h�r inst�llningar kopplar ihop denna appen med min app som �r skapad p� developers.facebook.com
                        options.AppId = "1011233316074653";
                        options.AppSecret = "fe447e4272d66875b000a9542fdf6aed";
                    });
                    //
                    //.AddGoogle(options =>
                    //{
                    //    //De h�r inst�llningar kopplar ihop denna appen med min app som �r skapad p� console.cloud.google.com
                    //    options.ClientId = "971180760685-1vb3tpujq1uj86qqk2j1e5dr5s55vn1c.apps.googleusercontent.com";
                    //    options.ClientSecret = "IOnpecEw7Vvria3kY0gVQQaB";
                    //    options.CallbackPath = "/Validation/GoogleSignIn";
                    //    options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                    //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();
            services.AddSession();
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
