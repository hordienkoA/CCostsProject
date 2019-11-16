using CConstsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCostsProject.Controllers
{
    [Route("api/[controller]")]
    public class TestController:Controller
    {
        DbWorker worker;
        
        public TestController()
        {

        }
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"Your login is : {User.Identity.Name}");
        }
        [HttpGet("ClearDb")]
        public IActionResult Get()
        {
            
        }
    }
}
