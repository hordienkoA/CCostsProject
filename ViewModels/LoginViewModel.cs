using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCostsProject.Models
{

    
    public class LoginViewModel
    {
        
        public string username { get; set; }
        public string password { get; set; }
    }
}
