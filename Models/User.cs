﻿using CCostsProject.Models;
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
        [Required]
        
        public string FirstName { get; set; }
        
        [Required]
        public string SecondName { get; set; }
        
        [JsonIgnore]
        public string Position { get; set; } 
        
        
        public double CashSum { get; set; } 
        [JsonIgnore]
        public int? FamilyId { get; set; }
        [JsonIgnore]
        public Family Family { get; set; }
        [JsonIgnore]
        public List<Outgo> Outgoes { get; set; }
        [JsonIgnore]
        public List<Income> Incomes { get; set; }
        
        public int? CurrencyId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
        [JsonIgnore]
        public int? FieldInfoId { get; set; }
        
        [JsonIgnore]
        public FileInfo  Avatar { get; set; }
        
        public User()
        {
            Outgoes = new List<Outgo>();
            Incomes = new List<Income>();
        }
    }
}
