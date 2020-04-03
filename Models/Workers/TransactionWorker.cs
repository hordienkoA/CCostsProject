using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class TransactionWorker:IWorker
    {
        private ApplicationContext db;

        public TransactionWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id == null ? db.Transactions.Include(t => t.Item)
                    .Include(t => t.Files)
                    .Include(t => t.User)
                    .Include(t=>t.Currency)
                    .FirstOrDefault() 
                : db.Transactions.Include(o => o.Item)
                    .Include(t => t.Files)
                    .Include(t => t.User)
                    .Include(t=>t.Currency)
                    .FirstOrDefault(t => t.Id == id);
        }

        public List<ITable> GetEntities()
        {
            return db.Transactions.Include(t => t.Item)
                .Include(t => t.Files)
                .Include(t => t.User)
                .Include(t=>t.Currency).ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Transactions.Add((Transaction) entity);
                db.SaveChanges();
            }
        }

        public void EditEntity(ITable entity)
        {
            var newTransaction = entity as Transaction;
            if (newTransaction != null)
            {
                var transaction = db.Transactions
                    .Include(t => t.Item)
                    .Include(t => t.Files).
                    FirstOrDefault(t=>t.Id==entity.Id);
                var newFiles = newTransaction.Files.Except(transaction.Files);
                var deletedFiles = transaction.Files.Except(newTransaction.Files);
                if (newFiles.Count() > 0)
                {
                    transaction.Files.AddRange(newFiles);
                }

                if (deletedFiles.Count() > 0)
                {
                    transaction.Files.RemoveAll(f => deletedFiles.Contains(f));
                }

                db.SaveChanges();

            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.Transactions.Remove(entity as Transaction ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}