
using BillingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BillingApp.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            string[] roles = { "SuperAdmin", "Admin", "Agent" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Role {role} created successfully.");
                }
                else
                {
                    logger.LogInformation($"Role {role} already exists.");
                }
            }

            string email = "mehakpreet@gmail.com";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var superAdmin = new User
                {
                    UserName = email,
                    Email = email,
                    FullName = "Super Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdmin, "Mehak@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                    logger.LogInformation("Super Admin user created and assigned role.");
                }
                else
                {
                    logger.LogError("Error creating Super Admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Super Admin already exists.");
            }
        }
        //    public static async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        //    {
        //        using var scope = serviceProvider.CreateScope();
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        //        string[] roles = { "SuperAdmin", "Admin", "Agent" };
        //        foreach (var role in roles)
        //        {
        //            if (!await roleManager.RoleExistsAsync(role))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }
        //        }


        //        string email = "mehakpreet@gmail.com";
        //        if (await userManager.FindByEmailAsync(email) == null)
        //        {
        //            var superAdmin = new User
        //            {
        //                UserName = email,
        //                Email = email,
        //                FullName = "Super Admin",
        //                EmailConfirmed = true
        //            };

        //            var result = await userManager.CreateAsync(superAdmin, "Mehak@123");
        //            if (result.Succeeded)
        //            {
        //                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        //            }
        //        }
        //    }
        //}
    }
}
