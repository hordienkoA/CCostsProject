﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCostsProject.Models;

namespace CConstsProject.Models
{
    public class Family:ITable
    {
     
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name{ get; set; }
        public string AdditionalInfo { get; set; }
        [Obsolete]
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [Obsolete]
        [JsonIgnore]
        public List<User> Users { get; set; }
        public Family()
        {
            Users = new List<User>();
        }
    }
}