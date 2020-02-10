using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCostsProject.Models;

namespace CConstsProject.Models
{
    public class Item:ITable
    {
        
        [Key]
        
        public int Id { get; set; }
        
        public double AvarageCost { get; set; }
        [Required]
        
        public string Type { get; set; }

        [JsonIgnore]
        public List<Outgo> Outgos { get; set; }
        public Item()
        {
            Outgos = new List<Outgo>();
        }
    }
}
