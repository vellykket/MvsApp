using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using MvsApplication.Models;

namespace MvsApp.Models
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<MvsAppContext>());
            }
        }

        public static void SeedData(MvsAppContext context)
        {
            System.Console.WriteLine("Appling Migrations...");
            context.Database.Migrate();
            if (!context.Users.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");
                context.Users.AddRange(
                    new User()
                    {
                        Firstname = "Vlad",
                        Lastname = "Stadnik",
                        Email = "email@gmail.com",
                        Password = "pass",
                        Accounts = new List<Account>(){new Account()
                        {
                            BalanceName = "MonobankStadnik",
                            BalanceNumber = "1234567890101112",
                            Balance = 0
                        }}
                    },
                    new User()
                    {
                        Firstname = "Yaroslav",
                        Lastname = "Gordienko",
                        Email = "email2@gmail.com",
                        Password = "paas",
                        Accounts = new List<Account>(){new Account()
                        {
                            BalanceName = "MonobankGordienko",
                            BalanceNumber = "1211100987654321",
                            Balance = 0
                        }}
                    }
                    );
                context.SaveChanges();
                System.Console.WriteLine("Data seeded. Success.");
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}