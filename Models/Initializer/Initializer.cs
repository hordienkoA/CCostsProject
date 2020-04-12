using System.Collections.Generic;
using System.Linq;
using CConstsProject.Controllers;
using CConstsProject.Models;

namespace CCostsProject.Models
{
    public class Initializer:IInitializer
    {
        private readonly ApplicationContext db;
        public Initializer(ApplicationContext context)
        {
            db = context;
        }
        public void CheckAndInitialize()
        {
            if (!db.Users.Any()||db.Users.FirstOrDefault(u=>u.UserName.Equals("Admin"))==null)
            {

                var Admin = new User
                {
                    UserName = "Admin", FirstName = "Johny ", SecondName = "Sins", Email = "baldfrombrazzers@pussy.com",
                    Password = "Admin", Position = "Admin"
                };
                
                db.Users.AddRange(Admin);
                
            }
            if (!db.Currencies.Any())
            {
                db.Currencies.Add(new Currency() { Name = "USD" });
                db.Currencies.Add(new Currency() { Name = "UAH" });
               
            }
            if (!db.Items.Any())
            {
                db.Items.Add(new Item { Name = "Food",CurrencyId = 1});
                db.Items.Add(new Item { Name = "Games",CurrencyId = 1,});
                
            }

            db.SaveChangesAsync();
        }
    }
}