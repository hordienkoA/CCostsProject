using CCostsProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using FileInfo = CCostsProject.Models.FileInfo;

namespace CConstsProject.Models
{
    public  class Outgo:IMoneySpent,ITable
    {
        [Key]
        public int Id { get; set; }
        public double Money { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        
        public int? ItemId { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        //[ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
        [JsonIgnore]
        public List<FileInfo> Files { get; set; }

        public Outgo()
        {
            Files=new List<FileInfo>();
        }
    }
}