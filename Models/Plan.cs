using System;
using System.ComponentModel.DataAnnotations;
using CConstsProject.Models;
using Newtonsoft.Json;

namespace CCostsProject.Models
{
    public class Plan:ITable
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public double Money { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateFinish { get; set; }
        [Required]
        public string Status { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
    }
}