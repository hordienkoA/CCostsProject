using System.Collections.Generic;

namespace CConstsProject.Models
{
    public class Family
    {
        
        public int Id { get; set; }
        public string AdditionalInformation { get; set; }
        public List<User> Users { get; set; }
        public Family()
        {
            Users = new List<User>();
        }
    }
}