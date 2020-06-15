using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCostsProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CConstsProject.Models
{
    public class Item:ITable
    {
        
        [Key]
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Percent { get; set; }
        public int AmountOfOutgoes { get; set; }
       // [Required]
        public int? CurrencyId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public Currency Currency { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public double AmountOfMoney { get; set; }

        [Obsolete]
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        [Obsolete]

        public User User { get; set; }

        [Obsolete]
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        public Item()
        {
            Transactions = new List<Transaction>();
        }
    }
}
