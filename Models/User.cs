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
        [Obsolete]
        public string Position { get; set; }
        [Obsolete]
        [JsonIgnore]
        public string Salt { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public double Money { get; set; }
        [Obsolete]
        [JsonIgnore]
        public int? FamilyId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public Family Family { get; set; }
        [Obsolete]
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        [Obsolete]
        [JsonIgnore]
        public List<Item> Items { get; set; }
        [Obsolete]
        [JsonIgnore]
        public List<Plan> Goals { get; set; }
        public int? CurrencyId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public Currency Currency { get; set; }
        [Obsolete]
        [JsonIgnore]
        public int? FieldInfoId { get; set; }
        [Obsolete]
        [JsonIgnore]
        public FileInfo  Avatar { get; set; }
        
    }
}
