using CConstsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCostsProject.Models
{
    public interface IMoneySpent
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        double Money { get; set; }
        User User { get; set; }
    }
}
