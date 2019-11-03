using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zhoplix.Models.Identity;
using Zhoplix.Services.ProfileManager;

namespace Zhoplix.Data
{
    public class DataInitializer
    {

        public  static void Initialize(IServiceProvider serviceProvider, ILogger logger)
        {

            SeedRoles(serviceProvider.GetService<RoleManager<IdentityRole<int>>>(), logger);
            SeedUsers(serviceProvider.GetService<UserManager<User>>(), serviceProvider.GetService<IProfileManager>(), logger);
        }

        public static void SeedRoles(RoleManager<IdentityRole<int>> roleManager, ILogger logger)
        {
            string[] roleNames = new[] { "Admin", "Moderator", "Member" };

            foreach (var roleName in roleNames)
            {
                var isRoleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!isRoleExist)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole<int>(roleName)).Result;
                    logger.LogInformation($"Create {roleName}: {roleResult.Succeeded}");
                }
            }
        }

        public static void SeedUsers(UserManager<User> userManager, IProfileManager profileManager, ILogger logger)
        {
            var superUser = new User
            {
                UserName = "Admin",
                Email = "azylav@gmail.com",
                EmailConfirmed = true
            };
            var user = userManager.FindByNameAsync(superUser.UserName).Result;
            if (user is null)
            {
                var result = userManager.CreateAsync(superUser, "Qwerty1").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(superUser, "Admin").Wait();
                    profileManager.CreateProfileAsync(superUser.Id).Wait();
                    logger.LogInformation($"Create Admin: Success");
                }
                else
                {
                    logger.LogError($"Create Admin: {result.Errors}");
                }

            }
        }
    }
}



