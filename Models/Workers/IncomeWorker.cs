using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class IncomeWorker:IWorker
    {
        private ApplicationContext db;
        
        public IncomeWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id == null ? db.Incomes
                .Include(i => i.Currency)
                .Include(i => i.Files)
                .Include(i => i.User)
                .FirstOrDefault() 
                : db.Incomes
                 .Include(i => i.Currency)
                .Include(i => i.Files)
                .Include(i => i.User)
                 .FirstOrDefault(i=>i.Id==id);
        }

        
        public List<ITable> GetEntities()
        {
            return db.Incomes
                .Include(i => i.Currency)
                .Include(i => i.Files)
                .Include(i => i.User)
                .ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Incomes.Add((Income)entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newIncome = entity as Income;
            if (newIncome != null)
            {
                var income = db.Incomes
                    .Include(i => i.Files)
                    .FirstOrDefault(i=>i.Id==entity.Id);
                income.Date = newIncome.Date;
                income.Description = newIncome.Description;
                income.IncomeType = newIncome.IncomeType;
                income.WorkType = newIncome.WorkType;
                var newFiles = newIncome.Files.Except(income.Files);
                var deletedFiles = income.Files.Except(newIncome.Files);
                if (newFiles.Count() > 0)
                {
                    income.Files.AddRange(newFiles);
                }

                if (deletedFiles.Count() > 0)
                {
                    income.Files.RemoveAll(f => deletedFiles.Contains(f));
                }

                db.SaveChanges();


            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Incomes.Remove(entity as Income ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}