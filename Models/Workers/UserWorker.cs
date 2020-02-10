using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class UserWorker:IWorker
    {
        private ApplicationContext db;
        public UserWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id==null? db.Users
                .Include(u => u.Avatar)
                .Include(u => u.Family)
                .Include(u=>u.Currency)
                .FirstOrDefault()
                :db.Users
                    .Include(u => u.Avatar)
                    .Include(u => u.Family)
                    .Include(u=>u.Currency)
                    .FirstOrDefault(f => f.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Users.Include(u => u.Avatar)
                .Include(u => u.Family)
                .Include(u=>u.Currency)
                .ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                var user = entity as User;
            if (db.Users.Any(u => u.Email == user.Email || u.UserName == user.UserName))
            {
                throw new NullReferenceException();
            }
            
                db.Users.Add((User) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newUser = entity as User;
            if (newUser != null)
            {
                var user = db.Users.Include(u => u.Avatar)
                    .Include(u => u.Family)
                    .FirstOrDefault(u => u.Id == entity.Id);
                user.Avatar = newUser.Avatar;
                user.Email = newUser.Email;
                user.Family = newUser.Family;
                user.Currency = newUser.Currency;
                user.Password = newUser.Password;
                user.Position = newUser.Position;
                user.FirstName = newUser.FirstName;
                user.SecondName = newUser.SecondName;
                user.UserName = newUser.UserName;

                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Users.Remove(entity as User ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}