using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CConstsProject.Models
{
    public class Family
    {
     
        [Key]
        public int Id { get; set; }
        public string AdditionalInformation { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
        public Family()
        {
            Users = new List<User>();
        }
    }
}