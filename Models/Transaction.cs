using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CConstsProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCostsProject.Models
{
    public class Transaction:ITable,IMoneySpent
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public User User { get; set; }
        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public double Money { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MMMM-dd'T'HH:mm:ss'Z'}",ApplyFormatInEditMode = true)]

        public DateTime Date { get; set; }
        
        
        public TransactionType? Type { get; set; }
        public string Description { get; set; }
        public int? ItemId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public Item Item { get; set; }
        
        
        public WorkType? WorkType { get; set; }
        public int? CurrencyId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public Currency Currency { get; set; }
        [Obsolete]
        [JsonIgnore]
        public List<FileInfo> Files { get; set; }
    }
}