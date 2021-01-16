using CaffeMenuBot.AppHost.Authentication;
using CaffeMenuBot.AppHost.Options;
using CaffeMenuBot.Data;
using CaffeMenuBot.Data.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CaffeMenuBot.AppHost
{
    public sealed class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<CaffeMenuBotContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("CaffeMenuBotDb"), builder =>
                    builder.EnableRetryOnFailure()
                        .MigrationsAssembly("CaffeMenuBot.Data")
                        .MigrationsHistoryTable("__MigrationHistory", CaffeMenuBotContext.SchemaName)));

            services.AddScoped<IAuthService, DatabaseBasedAuthService>();

            services.AddScoped<IDashboardRepo, PostgreDashboardRepo>();

            services.Configure<JwtOptions>(_configuration.GetSection("Jwt"));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();

            app.UseRouting();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}