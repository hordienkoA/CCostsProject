using CCostsProject.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class User:ITable
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        [JsonIgnore]
        public string Position { get; set; } 
        [JsonIgnore]
        public string Salt { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public double Money { get; set; } 
        [JsonIgnore]
        public int? FamilyId { get; set; }
        [JsonIgnore]
        public Family Family { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        [JsonIgnore]
        public List<Item> Items { get; set; }
        [JsonIgnore]
        public List<Plan> Goals { get; set; }
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
        [JsonIgnore]
        public int? FieldInfoId { get; set; }
        [JsonIgnore]
        public FileInfo  Avatar { get; set; }
        
    }
}
