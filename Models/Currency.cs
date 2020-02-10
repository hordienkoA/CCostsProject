using CConstsProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCostsProject.Models
{
    public class Currency:ITable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
        [JsonIgnore]
        public List<Income> Incomes { get; set; }
        [JsonIgnore]
        public List<Outgo> Outgoes { get; set; }
        public Currency()
        {
            Users = new List<User>();
            Incomes = new List<Income>();
            Outgoes = new List<Outgo>();
        }

        
    }
}
