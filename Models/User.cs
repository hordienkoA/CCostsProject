using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public String FullName { get; set; }
        public string Position { get; set; }
        public string WelcomeString { get; set; }
        public int? FamilyId { get; set; }
        public double CashSum { get; set; }
        public Family Family { get; set; }
        public List<Outgo> Outgoes { get; set; }
        public List<Income> Incomes { get; set; }
        public User()
        {
            Outgoes = new List<Outgo>();
            Incomes = new List<Income>();
        }
    }
}
