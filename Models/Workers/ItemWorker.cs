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
                .Include(i => i.Transactions).Include(i => i.User).FirstOrDefault() 
                : db.Items
                    .Include(i => i.Transactions)
                    .FirstOrDefault(i => i.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Items.Include(i => i.Transactions).Include(i=>i.User).ToList<ITable>();
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

                item.Name = newItem.Name;
                
                
                
                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Items.Remove(entity as Item ?? throw new NullReferenceException());
            db.SaveChanges();
        }

        public void ManageItemData(ITable entity,double money,bool inc)
        {
            var newItem = entity as Item;
            int itemsOutgoSum=0;
            if (newItem != null)
            {
                var item = db.Items.FirstOrDefault(i => i.Id == entity.Id);
                item.AmountOfOutgoes=inc?item.AmountOfOutgoes+1:item.AmountOfOutgoes-1;
                foreach (var VARIABLE in db.Items)
                {
                    itemsOutgoSum += VARIABLE.AmountOfOutgoes;
                }

                foreach (var i in db.Items)
                {
                    i.Percent = (int)(((double)i.AmountOfOutgoes/(double)itemsOutgoSum)*100);

                }

                
                item.AmountOfMoney=inc? item.AmountOfMoney+money:item.AmountOfMoney-money;
                db.SaveChanges();
            }
        }
    }
}