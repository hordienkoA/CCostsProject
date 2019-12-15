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
    public class Income:IMoneySpent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WorkType { get; set; }
        [Required]
        public double Money { get; set; }

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
       // [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
    }
}
