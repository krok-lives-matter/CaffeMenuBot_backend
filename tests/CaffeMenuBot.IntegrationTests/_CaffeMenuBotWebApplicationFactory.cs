﻿using System;
using System.Linq;
using CaffeMenuBot.AppHost.Configuration;
using CaffeMenuBot.Data;
using CaffeMenuBot.Data.Models.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CaffeMenuBot.IntegrationTests
{
    public sealed class CaffeMenuBotWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public CaffeMenuBotWebApplicationFactory()
        {
            ClientOptions.BaseAddress = new Uri("http://localhost:5001/");
            ClientOptions.AllowAutoRedirect = false;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {               
                ReplaceDbContext(services);

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CaffeMenuBotContext>();
                UserManager<DashboardUser> userManager = scopedServices.GetRequiredService<UserManager<DashboardUser>>();
                RoleManager<IdentityRole> roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CaffeMenuBotWebApplicationFactory<TStartup>>>();

                db.Database.EnsureCreated();

                try
                {
                    DatabaseSeeder.SeedDatabaseAsync(db, userManager, roleManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test data. Error: {Message}", ex.Message);
                }
            });
            builder.UseUrls("http://localhost:5001/");
        }

        private void ReplaceDbContext(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault
                (
                    d => d.ServiceType == typeof(DbContextOptions<CaffeMenuBotContext>)
                );

            services.Remove(descriptor);

            services.AddDbContext<CaffeMenuBotContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        }
    }
}