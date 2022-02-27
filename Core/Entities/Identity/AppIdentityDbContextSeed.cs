using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed",
                    Email = "Ahmed@store.com",
                    UserName = "Ahmed.Salama",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Salama",
                        Street = "Maddi",
                        State = "cairo",
                        Zipcode = "112265"

                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
