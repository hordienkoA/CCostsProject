using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class OutgoWorker:IWorker
    {
        private ApplicationContext db;
        public OutgoWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id == null ? db.Outgos.Include(o => o.Item)
                .Include(o => o.Files)
                .Include(o => o.User)
                .FirstOrDefault() 
                : db.Outgos.Include(o => o.Item)
                    .Include(o => o.Files)
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Outgos.Include(o => o.Item)
                .Include(o => o.Files)
                .Include(o => o.User).ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Outgos.Add((Outgo) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newOutgo = entity as Outgo;
            if (newOutgo != null)
            {
                var outgo = db.Outgos
                    .Include(o => o.Files)
                    .FirstOrDefault(o => o.Id == entity.Id);
                var newFiles = newOutgo.Files.Except(outgo.Files);
                var deletedFiles = outgo.Files.Except(newOutgo.Files);
                if (newFiles.Count() > 0)
                {
                    outgo.Files.AddRange(newFiles);
                }

                if (deletedFiles.Count() > 0)
                {
                    outgo.Files.RemoveAll(f => deletedFiles.Contains(f));
                }

                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Outgos.Remove(entity as Outgo ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}