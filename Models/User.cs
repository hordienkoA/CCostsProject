using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public String FullName { get; set; }
        [JsonIgnore]
        public string Position { get; set; } 
        public string WelcomeString { get; set; }
        
        public double CashSum { get; set; } 
        [JsonIgnore]
        public int? FamilyId { get; set; }
        [JsonIgnore]
        public Family Family { get; set; }
        [JsonIgnore]
        public List<Outgo> Outgoes { get; set; }
        [JsonIgnore]
        public List<Income> Incomes { get; set; }
        public User()
        {
            Outgoes = new List<Outgo>();
            Incomes = new List<Income>();
        }
    }
}
