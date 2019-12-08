using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WorkType { get; set; }
        [Required]
        public double Money { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
