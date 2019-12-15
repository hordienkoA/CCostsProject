using CCostsProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class DbWorker
    {
        ApplicationContext db;
        List<String> listOfEntities;
        Regex emailValidator;
        public DbWorker(ApplicationContext context)
        {
            db = context;
             listOfEntities = new List<string> { "Families", "Incomes", "Items", "Outgos", "TaskManagers", "Tasks", "Users" };
          emailValidator = new Regex(@"[A - Za - z0 - 9._ % +-] +@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
        }
        public double? MakeIncome(string  username,double money)
        {
            User user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                user.CashSum += money;
                db.SaveChanges();
                return user.CashSum;
            }
            return null;
        }
        public double? MakeOutgo(string username, double money)
        {
            User user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                user.CashSum -= money;
                db.SaveChanges();
                return user.CashSum;
            }
            return null;
        }
        public void AddOutgo(Outgo outgo)
        {
            if (outgo != null)
            {
                //outgo.Item.Outgos.Add(outgo);
                outgo.Item = db.Items.FirstOrDefault(i=>i.Id==outgo.ItemId);
                db.Outgos.Add(outgo);
                db.SaveChanges();
            }
        }
        public void DeleteOutgo(int id)
        {
            
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null)
            {
                db.Outgos.Remove(outgo);
                db.SaveChanges();
            }
            
        }
        public List<Outgo> GetOutgoes(User user)
        {
            return db.Outgos.Include(i => i.Item).Include(i => i.User).ToList();
        }
        public void EditOutgo(Outgo outg)
        {

            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == outg.Id);
            if (outgo != null)
            {
                outgo.ItemId = outg.ItemId;
                outgo.CurrencyId = outg.CurrencyId;
                outgo.Money = outg.Money;
                outgo.Date = outg.Date;
                outgo.Type = outg.Type;
                outgo.Description = outg.Description;
               
                db.SaveChanges();
            }
            
        }
        public void AddItem(double AverageCost,string Type)
        {
            db.Items.Add(new Item() { AvarageCost = AverageCost, Type = Type });
            db.SaveChanges();
        }
        public void DeleteItem(int id)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item != null) {
                db.Items.Remove(item);
                db.SaveChanges();
           }
        }
        public bool EditItem(int id,double? AvarageCost,string Type)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.AvarageCost = AvarageCost==null?item.AvarageCost:(double)AvarageCost;
                item.Type = Type;
                db.SaveChanges();
                return true;
            }
            return false;

        }
        public List<Item> GetItems()
        {
            return db.Items.Include(o => o.Outgos).ToList();
        }
        public void AddIncome(Income income)
        {
            if (income != null)
            {
                db.Incomes.Add(income);
                db.SaveChanges();
                
            }
        }
        public void EditIncome(Income inc)
        {

            Income income = db.Incomes.FirstOrDefault(i => i.Id == inc.Id);
            if (income != null)
            {
                income.WorkType = inc.WorkType;
                income.Date = inc.Date;
                income.Description = inc.Description;
                income.IncomeType = inc.IncomeType;
                income.Money = inc.Money;
                income.CurrencyId = inc.CurrencyId;
                db.SaveChanges();
            }
        }
        public void DeleteIncom(int id)
        {
            Income income = db.Incomes.FirstOrDefault(o => o.Id == id);
            if (income != null)
            {
                db.Remove(income);
                db.SaveChanges();
            }
        }
        public List<Income> GetIncomes()
        {
            return db.Incomes.Include(u=>u.User).ToList();
        }

        public void AddTask(Task task)
        {
            if (task != null)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }

        }
        public void EditTask(int id,string True_rule,string False_rule)
        {
            Task task = db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.True_rule = True_rule;
                task.False_rule = False_rule;
                db.SaveChanges();
            }
        }
        public void ClearDb()
        {
            db.Database.ExecuteSqlCommand("Delete from Items;DBCC CHECKIDENT('Items', RESEED, 0)");
            db.Database.ExecuteSqlCommand("Delete from Incomes;DBCC CHECKIDENT('Incomes', RESEED, 0)");
            db.Database.ExecuteSqlCommand("Delete from Outgos;DBCC CHECKIDENT('Outgos', RESEED, 0)");
            db.Database.ExecuteSqlCommand("Delete from Tasks;DBCC CHECKIDENT('Tasks', RESEED, 0)");
            db.Database.ExecuteSqlCommand("Delete from TaskManagers;DBCC CHECKIDENT('TaskManagers', RESEED, 0)");
            db.Database.ExecuteSqlCommand("Delete from Users;DBCC CHECKIDENT('Users', RESEED, 0)");
            
            
           
            
            

        }
        public bool AddUser(User user)
        {
            if (db.Users.Any(u => u.Email == user.Email || u.UserName == user.UserName))
            {
                return false;
            }
            //else if (ValidateUser(user))
            //{
            //    return false;
            //}
            db.Users.Add(user);
            db.SaveChanges();
            return true;
        }
        public User GetUser(int? id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public Outgo GetOutgo(int? id)
        {
            return db.Outgos.FirstOrDefault(o => o.Id == id);
        }

        public Income GetIncome(int? id)
        {
            return db.Incomes.FirstOrDefault(i => i.Id == id);
        }
        public  Income GetLastIncome()
        {
             return  db.Incomes.Last();
        }

        public Outgo GetLastOutgo()
        {
            return db.Outgos.Last();
        }
        
        public Item GetLastItem()
        {
            return db.Items.Last();
        }
        public User GetLastUser()
        {
            return db.Users.Last();
        }
        public Currency GetLastCurrency()
        {
            return db.Currencies.LastOrDefault();
        }
        public Family GetFamily(int id)
        {
            return db.Families.FirstOrDefault(f => f.Id == id);
        }
        public Currency GetCurrency(int id)
        {
            return db.Currencies.FirstOrDefault(c => c.Id == id);
        }
        public List<Currency> GetCurrencies()
        {
            return db.Currencies.ToList();
        }
        public Family GetFamilyByUserName(string username)
        {
            User user = db.Users.FirstOrDefault(u => u.UserName == username);
            return db.Families.Include(f => f.Users).FirstOrDefault(f => f.Users.Contains(user));
        }

        public bool ValidateUser(User user)
        {

            return user.Email.Length < 3 || user.Email.Length > 64 ||user.Password.Length<8
                ||user.Password.Length>255||user.UserName.Length<3||user.UserName.Length>64||user.FullName.Length<1||user.FullName.Length>255;
        }

        public List<Income> GetIncomesByDateRange(DateTime fromDate,DateTime? toDate)
        {
            return toDate == null ? db.Incomes.Include(i => i.User).Where(i => i.Date > fromDate && i.Date < DateTime.Now).ToList<Income>() : db.Incomes.Where(i => i.Date > fromDate && i.Date < toDate).ToList<Income>();
        }

        public List<Outgo> GetOutgoesByDataRange(DateTime fromDate,DateTime? toDate)
        {
            return toDate == null ? db.Outgos.Include(o => o.User).Where(o => o.Date > fromDate && o.Date < DateTime.Now).ToList<Outgo>() : db.Outgos.Where(o => o.Date > fromDate && o.Date < toDate).ToList<Outgo>();
        }

        public List<IMoneySpent> GetOuthoesAndIncomesByDateRange(DateTime fromDate,DateTime? toDate)
        {
            List<IMoneySpent> list = new List<IMoneySpent>();
            if (toDate == null)
            {
                list.AddRange(db.Incomes.Include(i => i.User).Where(i => i.Date > fromDate && i.Date < DateTime.Now));
                list.AddRange(db.Outgos.Include(o => o.User).Where(o => o.Date > fromDate && o.Date < DateTime.Now));
            }
            else
            {
                list.AddRange(db.Incomes.Include(i=>i.User).Where(i => i.Date > fromDate && i.Date < toDate));
                list.AddRange(db.Outgos.Include(o => o.User).Where(o => o.Date > fromDate && o.Date < toDate));
            }
            return list;
        }

        public bool AddCurrency(Currency currency)
        {
            if (currency != null&&!db.Currencies.Any(c=>c.Name==currency.Name))
            {
                db.Currencies.Add(currency);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool EditCurrency(int id,string name,out Currency outCurrency)
        {
            Currency cur = db.Currencies.Include(i=>i.Incomes).Include(o=>o.Outgoes).Include(u=>u.Users).FirstOrDefault(c=>c.Id==id);
            if (cur != null)
            {
                cur.Name = name;
                db.SaveChanges();
                outCurrency = cur;
                return true;
            }
            outCurrency = null;
            return false;
        }
        public bool DeleteCurrency(int id)
        {
            Currency cur = db.Currencies.Include(i => i.Incomes).Include(o => o.Outgoes).Include(u => u.Users).FirstOrDefault(c => c.Id == id);
            if (cur.Incomes.Count == 0 && cur.Users.Count==0 && cur.Outgoes.Count==0)
            {
                db.Currencies.Remove(cur);
                db.SaveChanges();
                return true;
            }
            return false;

        }

    }
}
