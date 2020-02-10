using System.Linq;
using CConstsProject.Models;

namespace CCostsProject.Models
{
    public class CashManager : ITransactionManager
    {
        private ApplicationContext db;

        public CashManager(ApplicationContext context)
        {
            db = context;
        }

        public void execute(string username, double money)
        {
            User user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                user.CashSum += money;
                db.SaveChanges();

            }

        }

        public void undo(string username, double money)
        {
            User user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                user.CashSum -= money;
                db.SaveChanges();
            }
        }
    }
}
