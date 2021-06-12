using EcommerceMVC.Auth;
using EcommerceMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EcommerceMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("connection_string");

            services.AddIdentity<ApplicationUser, AppRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(connectionString);
            });

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddMicrosoftAccount((config) =>
            {
                config.ClientId = "03333af0-3fbc-4968-b720-c0ed8427b070";
                config.ClientSecret = "b13906c7-0afe-4a6f-8b4d-1fb8e49e2678";
                config.TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize/";
            })
                .AddGoogle((config) =>
                {
                    config.ClientId = "595320647233-v86smi4ahhm9qpt9rluv81knlprkge0e.apps.googleusercontent.com";
                    config.ClientSecret = "0lKzAZ6aixx63kLFBD0mK15B";
                })
                .AddCookie(config => config.SlidingExpiration = true)
                .AddJwtBearer("Bearer", cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.SaveToken = true;
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidIssuer = JwtValues.Issuer,
                       ValidAudience = JwtValues.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtValues.Key))
                   };
               });

            services.AddControllersWithViews();

            services.AddSingleton<IJwtAuthManager>(new AuthManager());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}
