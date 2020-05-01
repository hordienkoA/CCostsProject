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
        public int r030 { get; set; }
        [Required]

        public string txt { get; set; }
        [Required]

        public double rate { get; set; }
        [Required]
        public string cc { get; set; } 
        [Required]

        public string exchangedate { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        
        [JsonIgnore]
        public List<Plan> Plans { get; set; }

        public Currency()
        {
            Users = new List<User>();
            Transactions = new List<Transaction>();
            
        }

        
    }
}
