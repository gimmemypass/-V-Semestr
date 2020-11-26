using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using _V_Semestr.Data;
using _V_Semestr.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using _V_Semestr.Data.FileManager;
using _V_Semestr.Configuration;
using _V_Semestr.Services.Email;
using _V_Semestr.Models;
using _V_Semestr.Hubs;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;

namespace _V_Semestr
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
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["DefaultConnection"]));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
//                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Auth/Login";
            });
            //services.AddAuthorization(option =>
            //{
            //    option.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            //});
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSignalR();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.CacheProfiles.Add("Monthly", new Microsoft.AspNetCore.Mvc.CacheProfile { Duration = 60 * 60 * 24 * 7 * 4 });
            })
                .AddViewLocalization();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }
                app.UseDeveloperExceptionPage();

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //get culture information based on web browser
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHub<CommentHub>("/commentHub");
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
