using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DAL.Data;
using DAL.Data.DataSeed;
using BLL.Service;
using Microsoft.AspNetCore.Identity;
using DAL.Data.Models.IdentityModels;

namespace Charity_BE.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using an in-memory database for testing.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });


            // Add DataSeed service
            services.AddScoped<DataSeed>();

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            // context (ApplicationDbContext).
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Create test users directly for testing
                    var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                    // Create roles
                    if (!roleManager.RoleExistsAsync("Admin").Result)
                        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                    if (!roleManager.RoleExistsAsync("Supervisor").Result)
                        roleManager.CreateAsync(new IdentityRole("Supervisor")).Wait();
                    if (!roleManager.RoleExistsAsync("Mediation").Result)
                        roleManager.CreateAsync(new IdentityRole("Mediation")).Wait();
                    if (!roleManager.RoleExistsAsync("Requester").Result)
                        roleManager.CreateAsync(new IdentityRole("Requester")).Wait();

                    // Create test users
                    var adminUser = new ApplicationUser
                    {
                        UserName = "Admin@gmail.com",
                        Email = "Admin@gmail.com",
                        FullName = "Test Admin",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(adminUser, "P@ssw0rd123").Wait();
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();

                    var supervisorUser = new ApplicationUser
                    {
                        UserName = "supervisor@gmail.com",
                        Email = "supervisor@gmail.com",
                        FullName = "Test Supervisor",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(supervisorUser, "P@ssw0rd123").Wait();
                    userManager.AddToRoleAsync(supervisorUser, "Supervisor").Wait();

                    var mediationUser = new ApplicationUser
                    {
                        UserName = "mediation@gmail.com",
                        Email = "mediation@gmail.com",
                        FullName = "Test Mediation",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(mediationUser, "P@ssw0rd123").Wait();
                    userManager.AddToRoleAsync(mediationUser, "Mediation").Wait();

                    // Create some basic reconcile request types
                    if (!db.ReconcileRequestTypes.Any())
                    {
                        db.ReconcileRequestTypes.AddRange(new[]
                        {
                            new DAL.Data.Models.ReconcileRequestType
                            {
                                Name = "Test Type 1",
                                Description = "Test Description 1",
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow
                            },
                            new DAL.Data.Models.ReconcileRequestType
                            {
                                Name = "Test Type 2",
                                Description = "Test Description 2",
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow
                            }
                        });
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                }
            }
        });
    }
}