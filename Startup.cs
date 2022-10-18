using FurStore.Data;
using FurStore.Mappings;
using FurStore.Models.Auth;
using FurStore.Services.FurnitureServices;
using FurStore.Services.OrderServices;
using FurStore.Services.SearchServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FurStore
{
    public class Startup
    {
        private readonly string _dbConnectionKey = "DbConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(AppDbContext).Assembly));
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString(_dbConnectionKey));
            });

            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IFurnitureService, FurnitureService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;  
                opts.Password.RequireNonAlphanumeric = false;  
                opts.Password.RequireLowercase = false; 
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false; 
            })
                .AddEntityFrameworkStores<AppDbContext>();

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            DbInitializer.Seed(app).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Furniture}/{action=Index}/{id?}");
            });
        }
    }
}
