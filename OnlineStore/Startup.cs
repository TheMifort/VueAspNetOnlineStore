using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStore.Areas.Admin;
using OnlineStore.Areas.Items;
using OnlineStore.Database;
using OnlineStore.Helpers;
using OnlineStore.Models.Database;

namespace OnlineStore
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
            services.AddControllers();

            services.AddSpaStaticFiles(options => options.RootPath = "client-app/dist");
            
            services.AddTransient<SeedContext>();

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

            services.AddAutoMapper(configAction: cfg =>
            {
                new ItemsMapperInit().MapperInit(cfg);
                new AdminMapperInit().MapperInit(cfg);
            }, Assembly.GetAssembly(GetType()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedContext seedContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                await seedContext.Seed(scope.ServiceProvider.GetService<DatabaseContext>());
            }

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";
                if (env.IsDevelopment())
                {
                    spa.UseVueDevelopmentServer();
                }
            });
        }
    }
}