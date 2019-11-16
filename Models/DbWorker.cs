using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class DbWorker
    {
        ApplicationContext db;
        List<String> listOfEntities;
        public DbWorker(ApplicationContext context)
        {
            db = context;
             listOfEntities = new List<string> { "Families", "Incomes", "Items", "Outgos", "TaskManagers", "Tasks", "Users" };

        }
        public void AddOutgo(double Money,DateTime Date,Item Item,User User)
        {
            Outgo outgo = new Outgo() { Money = Money, Date = Date, Item = Item, User = User };
            outgo.Item.Outgos.Add(outgo);
            db.Outgos.Add(outgo);
            db.SaveChanges();
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
        public void EditOutgo(int id,double Money,DateTime Date,Item item,User user)
        {

            Outgo outgo = db.Outgos.Include(i=>i.Item).Include(i=>i.User).FirstOrDefault(o => o.Id == id);
            if (outgo != null)
            {
                outgo.Item.Outgos.Remove(outgo);
                outgo.Item = item;
                outgo.Money = Money;
                outgo.User = user;
                outgo.Date = Date;
                outgo.Item.Outgos.Add(outgo);
               
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
        public void EditItem(int id,double? AvarageCost,string Type)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.AvarageCost = AvarageCost==null?item.AvarageCost:(double)AvarageCost;
                item.Type = Type;
                db.SaveChanges();
            }

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
        public void EditIncome(int id,string WorkType,DateTime Date)
        {

            Income income = db.Incomes.FirstOrDefault(o => o.Id == id);
            if (income != null)
            {
                income.WorkType = WorkType;
                income.Date = Date;
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
            return db.Incomes.ToList();
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
            foreach(var tableName in listOfEntities)
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [" + tableName + "]");
            }
        }
        
    }
}
