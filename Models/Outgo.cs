using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace CConstsProject.Models
{
    public  class Outgo
    {
        [Key]
        public int Id { get; set; }
        public double Money { get; set; }
        public DateTime Date { get; set; }
        [JsonIgnore]
        public int? ItemId { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

    }
}