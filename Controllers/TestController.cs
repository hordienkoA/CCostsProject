
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
        ApplicationContext db;
        
        public TestController(ApplicationContext context)
        {
            db = context;
            worker = new DbWorker(db);
        }
      
        [HttpDelete("ClearDb")]
        public IActionResult Delete()
        {
            worker.ClearDb();
            return Ok();
        }
    }
}

