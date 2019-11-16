using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string WorkType { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
