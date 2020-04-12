using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class FamilyWorker:IWorker
    {
        private ApplicationContext db;
        public FamilyWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id==null? db.Families.Include(f=>f.Users).FirstOrDefault():db.Families.Include(f=>f.Users).FirstOrDefault(f => f.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Families.Include(f=>f.Users).ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Families.Add((Family) entity);
              
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newFamily= entity as Family;
            if (newFamily != null)
            {
                var family = db.Families.Include(f=>f.Users).FirstOrDefault(f => f.Id == entity.Id);

                family.Name = newFamily.Name;
                family.AdditionalInfo = newFamily.AdditionalInfo;
                var newUsers = newFamily.Users.Except(family.Users);
                var deletedUsers = family.Users.Except(newFamily.Users);
                if (newUsers.Count() > 0)
                {
                    family.Users.AddRange(newUsers);
                }

                if (deletedUsers.Count() > 0)
                {
                    family.Users.RemoveAll(u => deletedUsers.Contains(u));
                    
                }
                db.SaveChanges();
                
            }
            
        }

        public void DeleteEntity(ITable entity)
        {
            
            db.Families.Remove(entity as Family ?? throw new NullReferenceException());
            db.SaveChanges();
        }
        public void AddUser(int familyId, int userId)
        {
            var family = db.Families.FirstOrDefault(f => f.Id == familyId);
            family.Users.Add(db.Users.FirstOrDefault(u=>u.Id==userId));
            db.SaveChanges();
        }
    }
}