
using BillingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BillingApp.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string[] roles = { "SuperAdmin", "Admin", "Agent" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
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
                }
            }
        }
    }
}
