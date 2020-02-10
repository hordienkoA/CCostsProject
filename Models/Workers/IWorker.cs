using System.Collections.Generic;

namespace CCostsProject.Models
{
    public interface IWorker
    {
        ITable GetEntity(int? id);
        List<ITable> GetEntities();
        void AddEntity(ITable entity);
        void EditEntity(ITable entity);
        void DeleteEntity(ITable entity);
        
    }
}