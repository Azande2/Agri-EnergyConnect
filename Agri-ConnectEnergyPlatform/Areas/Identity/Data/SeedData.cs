
using Agri_ConnectEnergyPlatform.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<Agri_ConnectEnergyPlatformUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure Employee role exists
        if (!await roleManager.RoleExistsAsync("Employee"))
        {
            await roleManager.CreateAsync(new IdentityRole("Employee"));
        }

        // Create predefined employees
        var predefinedEmployees = new[]
        {
            new { Email = "admin1@agrienergy.com", Password = "Employee@123" },
            new { Email = "azandemnguni@agrienergy.com", Password = "azande@123" },
            new { Email = "qhawe@agrienergy.com", Password = "qhawe@123" },
            new { Email = "tomreddy4@agrienergy.com", Password = "Tom@123" },
        };

        foreach (var emp in predefinedEmployees)
        {
            var user = await userManager.FindByEmailAsync(emp.Email);
            if (user == null)
            {
                var newUser = new Agri_ConnectEnergyPlatformUser
                {
                    UserName = emp.Email,
                    Email = emp.Email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, emp.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Employee");
                }
            }
        }
    }
}
