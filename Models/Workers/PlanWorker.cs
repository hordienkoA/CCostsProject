using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class PlanWorker:IWorker
    {
        private ApplicationContext db;

        public PlanWorker(ApplicationContext context)
        {
            db = context;
        }

        public ITable GetEntity(int? id)
        {
            return id == null
                ? db.Goals.Include(p => p.User).FirstOrDefault()
                : db.Goals.Include(p => p.User).FirstOrDefault(p => p.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Goals.Include(p => p.User).ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Goals.Add((Plan) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newPlan = entity as Plan;
            if (newPlan != null)
            {
                var plan = db.Goals.FirstOrDefault(p => p.Id == entity.Id);

                plan.Money = newPlan.Money;
                plan.DateStart = newPlan.DateStart;
                plan.DateFinish = newPlan.DateFinish;
                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Goals.Remove(entity as Plan ?? throw new NullReferenceException());
            db.SaveChanges();
        }

        public void ChangeStatus(string status, int id)
        {
            var plan = db.Goals.FirstOrDefault(p => p.Id == id);
            if (plan != null)
            {
                plan.Status = status;
                db.SaveChanges();
            }
        }
    }
}