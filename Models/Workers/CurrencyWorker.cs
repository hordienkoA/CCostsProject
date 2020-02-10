using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;

namespace CCostsProject.Models
{
    public class CurrencyWorker:IWorker
    {
        private ApplicationContext db;
        public CurrencyWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            
            return id==null?db.Currencies.FirstOrDefault():db.Currencies.FirstOrDefault(c=>c.Id==id);
        }

        public List<ITable> GetEntities()
        {
            return db.Currencies.ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            if (entity != null)
            {
                db.Currencies.Add((Currency)entity);
            }
        }

        public void EditEntity(ITable entity)
        {
            throw new System.NotSupportedException();
        }

        public void DeleteEntity(ITable entity)
        {
            throw new System.NotSupportedException();
        }
    }
}