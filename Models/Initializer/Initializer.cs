using System.Collections.Generic;
using System.Linq;
using CConstsProject.Controllers;
using CConstsProject.Models;
using Microsoft.Extensions.Configuration;

namespace CCostsProject.Models
{
    public class Initializer:IInitializer
    {
        private readonly ApplicationContext db;
        private readonly IConfiguration _config;
        public Initializer(ApplicationContext context,IConfiguration config)
        {
            db = context;
            _config = config;
        }
        public void CheckAndInitialize()
        {
            if (!db.Users.Any()||db.Users.FirstOrDefault(u=>u.UserName.Equals("Admin"))==null)
            {

                var salt = Salt.Create();
                var Admin = new User
                {
                    UserName = "Admin", FirstName = "Andrii", SecondName = "Hordiienko", Email = "gord34326@gmail.com",
                    Password = Hash.Create("Admin"+_config.GetValue<string>("GlobalParameter"),salt),Salt = salt,Position = "Admin",CurrencyId = 1
                };
                
                db.Users.AddRange(Admin);
                
            }
            db.SaveChanges();
            /*if (!db.Currencies.Any())
            {
                db.Currencies.Add(new Currency() { Name = "USD" });
                db.Currencies.Add(new Currency() { Name = "UAH" });
               
            }*/
            /*if (!db.Items.Any())
            {
                db.Items.Add(new Item { Name = "Food",CurrencyId = 1});
                db.Items.Add(new Item { Name = "Games",CurrencyId = 1,});
                
            }*/

         
        }
    }
}