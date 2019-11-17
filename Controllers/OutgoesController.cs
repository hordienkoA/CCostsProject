using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Controllers
{
    [Route("api/[controller]")]
    public class OutgoesController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public OutgoesController(ApplicationContext context)
        {
            db = context;
            Worker = new DbWorker(db);
        }
        
        [HttpPost("AddOutgo")]
        public  IActionResult Post([FromBody]Outgo outgo)
        {
            if (outgo != null)
            {
                Worker.AddOutgo(outgo);
               
                return Ok(outgo);
            }
            return Forbid();
        }
        [HttpPost("EditOutgo")]
        public IActionResult Post(int id,double Money,DateTime Date)
        {
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null)
            {
                outgo.Money = Money;
                outgo.Date = Date;
                db.SaveChanges();
                return Ok(outgo);
            }
            return Forbid();
        }
        [HttpDelete("DelelteOutgo")]
        public IActionResult Delete(int id)
        {
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null)
            {
                db.Outgos.Remove(outgo);
                db.SaveChanges();
                return Ok();
            }
            return Forbid();
        }
        [HttpGet("GetOutgoes")]
        public IActionResult GetOutgoes()
        {
            return new JsonResult(db.Outgos.Include(o=>o.Item).Include(o=>o.User).ToList());
        }
    }
}