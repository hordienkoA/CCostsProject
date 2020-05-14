using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;

namespace CCostsProject.Models
{
    public class InviteWorker:IWorker
    {
        private ApplicationContext db;
        public InviteWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id == null ? db.Invites.FirstOrDefault() : db.Invites.FirstOrDefault(i => i.Id == id);

        }

        public List<ITable> GetEntities()
        {
            return db.Invites.ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Invites.Add((Invite) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            throw new System.NotSupportedException();
        }

        public void DeleteEntity(ITable entity)
        {
            db.Invites.Remove(entity as Invite ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}