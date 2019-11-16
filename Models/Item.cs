using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class Item
    {
        
        public int Id { get; set; }
        public double AvarageCost { get; set; }
        public string Type { get; set; }
        public List<Outgo> Outgos { get; set; }
        public Item()
        {
            Outgos = new List<Outgo>();
        }
    }
}
