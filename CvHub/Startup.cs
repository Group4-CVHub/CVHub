using CVHub.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            // Denna service gör så att man kan kräva inloggning på vissa delar av hemsidan genom att kalla på [Authorize].
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                    .AddCookie(options =>
                    {
                        //Användaren skickas hit om åtkomsten nekas.
                        options.LoginPath = "/Validation/SignIn";
                    })
                    .AddFacebook(options =>
                    {
                        //De här inställningar kopplar ihop denna appen med min app som är skapad på developers.facebook.com
                        options.AppId = "1011233316074653";
                        options.AppSecret = "fe447e4272d66875b000a9542fdf6aed";
                    });
                    //
                    //.AddGoogle(options =>
                    //{
                    //    //De här inställningar kopplar ihop denna appen med min app som är skapad på console.cloud.google.com
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

            services.AddMvc().AddSessionStateTempDataProvider();

            services.AddSession();

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "CvHub API v1",
                   Description = "A simple example of the CvHub v1 API"
               });
           });
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

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CvHub v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
