/*using System;
using System.Collections.Generic;
using System.Linq;
using CConstsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Models
{
    public class StatisticMaker
    {
        private readonly ApplicationContext db;
        private readonly IWorker IncomeWork;
        private readonly IWorker OutgoWork;
        public StatisticMaker(ApplicationContext context)
        {
            db = context;
            IncomeWork=new IncomeWorker(db);
            OutgoWork=new OutgoWorker(db);
            
        }
        public List<Income> GetIncomesByDateRange(DateTime fromDate,DateTime? toDate)
        {
            if (toDate!=null&&fromDate > toDate )
            {
                var proxDate = fromDate;
                fromDate = (DateTime)toDate;
                toDate = proxDate;
            }
            return toDate == null ? IncomeWork.GetEntities().Cast<Income>().Where(i => i.Date > fromDate && i.Date < DateTime.Now).ToList() 
                : IncomeWork.GetEntities().Cast<Income>().Where(i => i.Date > fromDate && i.Date < toDate).ToList();
        }

        public List<Outgo> GetOutgoesByDataRange(DateTime fromDate,DateTime? toDate)
        {
            if (toDate!=null&&fromDate > toDate )
            {
                var proxDate = fromDate;
                fromDate = (DateTime)toDate;
                toDate = proxDate;
            }
            return toDate == null ? OutgoWork.GetEntities().Cast<Outgo>().Where(o => o.Date > fromDate && o.Date < DateTime.Now).ToList() : OutgoWork.GetEntities().Cast<Outgo>().Where(o => o.Date > fromDate && o.Date < toDate).ToList<Outgo>();
        }

        public List<IMoneySpent> GetOutgoesAndIncomesByDateRange(DateTime fromDate,DateTime? toDate)
        {
            if (toDate!=null&&fromDate > toDate )
            {
                var proxDate = fromDate;
                fromDate = (DateTime)toDate;
                toDate = proxDate;
            }
            List<IMoneySpent> list = new List<IMoneySpent>();
            if (toDate == null)
            {
                list.AddRange(IncomeWork.GetEntities().Cast<Income>().Where(i => i.Date > fromDate && i.Date < DateTime.Now));
                list.AddRange(OutgoWork.GetEntities().Cast<Outgo>().Where(o => o.Date > fromDate && o.Date < DateTime.Now));
            }
            else
            {
                list.AddRange(IncomeWork.GetEntities().Cast<Income>().Where(i => i.Date > fromDate && i.Date < toDate));
                list.AddRange(OutgoWork.GetEntities().Cast<Outgo>().Where(o => o.Date > fromDate && o.Date < toDate));
            }
            return list;
        }

    }
}*/