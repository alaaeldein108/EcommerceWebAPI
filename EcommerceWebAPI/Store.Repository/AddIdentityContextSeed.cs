using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AddIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Alaa Eldein",
                    Email = "alaa@gmail.com",
                    UserName="AlaaEldein",  
                    Address=new Address
                    {
                        City="Maadi",
                        State="Cario",
                        street="77",
                        ZipCode="12345"
                    },
                };
                await userManager.CreateAsync(user,"Pass123!");

            }
        }
    }
}
