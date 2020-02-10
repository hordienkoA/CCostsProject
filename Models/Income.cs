using CCostsProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class Income:IMoneySpent,ITable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WorkType { get; set; }
        [Required]
        public double Money { get; set; }

        [Required]
        public string IncomeType { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int? CurrencyId { get; set; }
        [JsonIgnore]
       
        public Currency Currency { get; set; }
        
        [JsonIgnore]
        public List<FileInfo> Files { get; set; }

        public Income()
        {
            Files=new List<FileInfo>();
        }
    }
}
