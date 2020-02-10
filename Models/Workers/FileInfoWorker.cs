using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class FileInfoWorker:IWorker
    {
        private ApplicationContext db;
        public FileInfoWorker(ApplicationContext context)
        {
            db = context;
        }
        public ITable GetEntity(int? id)
        {
            return id==null?db.FileInfos.FirstOrDefault():db.FileInfos.FirstOrDefault(c=>c.Id==id);
        }

        public List<ITable> GetEntities()
        {
            return db.FileInfos.ToList<ITable>();
        }

        public void AddEntity(ITable entity)
        {
            
            if (entity!=null)
            {
                db.FileInfos.Add((FileInfo)entity);
                db.SaveChanges();
            }
            
        }

        public void EditEntity(ITable entity)
        {
            var newFileInfo = entity as FileInfo;
            if (newFileInfo != null)
            {
                var fileInfo = db.FileInfos.FirstOrDefault(f => f.Id == entity.Id);

                fileInfo.Name = newFileInfo.Name;
                fileInfo.Path = newFileInfo.Path;
                db.SaveChanges();
            }
        }

        public void DeleteEntity(ITable entity)
        {
            db.FileInfos.Remove(entity as FileInfo ?? throw new NullReferenceException());
            db.SaveChanges();
        }
    }
}