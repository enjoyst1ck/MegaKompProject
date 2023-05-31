using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address 
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "Budowlanych 10",
                        City = "Opole",
                        Country = "Polska",
                        ZipCode = "45-460"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}