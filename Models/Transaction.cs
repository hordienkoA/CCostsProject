using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CConstsProject.Models;
using Newtonsoft.Json;

namespace CCostsProject.Models
{
    public class Transaction:ITable,IMoneySpent
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [Required]
        public double Money { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MMMM-dd'T'HH:mm:ss'Z'}",ApplyFormatInEditMode = true)]

        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? ItemId { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        public string WorkType { get; set; }
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
        [JsonIgnore]
        public List<FileInfo> Files { get; set; }
    }
}