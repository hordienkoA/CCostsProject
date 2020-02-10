using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class ItemWorker:IWorker
    {
        private ApplicationContext db;
        public ItemWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id == null ? db.Items
                .Include(i => i.Outgos).FirstOrDefault() 
                : db.Items
                    .Include(i => i.Outgos)
                    .FirstOrDefault(i => i.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Items.Include(i => i.Outgos).ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Items.Add((Item) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newItem = entity as Item;
            if (newItem != null)
            {
                var item = db.Items.FirstOrDefault(i => i.Id == entity.Id);

                item.Type = newItem.Type;
                item.AvarageCost = newItem.AvarageCost;
                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Items.Remove(entity as Item ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}