using Microsoft.AspNetCore.Identity;

namespace ShowTrack.Web.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task SeedData(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var adminUserName = configuration["AdminUserName"];
        var adminPassword = configuration["AdminPassword"];

        if (adminUserName is null || adminPassword is null)
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if(await roleManager.RoleExistsAsync("admin") == false)
        {
            await roleManager.CreateAsync(new() { Name = "admin" });
        }

        if (userManager.Users.Any(u => u.UserName == userManager.NormalizeName(adminUserName)))
        {
            return;
        }

        var adminUser = new IdentityUser
        {
            Email = adminUserName,
            UserName = adminUserName,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser);
        await userManager.AddPasswordAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, "admin");
    }
}
