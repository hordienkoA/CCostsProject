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
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MMMM-dd'T'HH:mm:ss'Z'}",ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy'-'MMMM-dd'T'HH:mm:ss'Z'}",ApplyFormatInEditMode = true)]

        public DateTime DateFinish { get; set; }
        /*[Required]*/
        public string Status { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
    }
}