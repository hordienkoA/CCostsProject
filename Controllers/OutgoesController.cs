using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize]
        [HttpPost("AddOutgo")]
        public  IActionResult Post([FromBody]Outgo outgo)
        {
            if (outgo != null)
            {
                outgo.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                Worker.AddOutgo(outgo);
                Worker.MakeOutgo(User.Identity.Name,outgo.Money);
                return Ok(outgo+"current cash:");
            }
            return Forbid();
        }
        [Authorize]
        [HttpPost("EditOutgo")]
        public IActionResult Post(int id,double Money,DateTime Date)
        {
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null&&outgo.User.UserName==User.Identity.Name)
            {
                outgo.Money = Money;
                outgo.Date = Date;
                db.SaveChanges();
                return Ok(outgo);
            }
            return Forbid();
        }
        [Authorize]
        [HttpDelete("DelelteOutgo")]
        public IActionResult Delete(int id)
        {
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null && outgo.User.UserName == User.Identity.Name)
            {
                db.Outgos.Remove(outgo);
                db.SaveChanges();
                return Ok();
            }
            return Forbid();
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Outgo outgo = Worker.GetOutgo(id);
            if (outgo != null)
            {
                return Json(outgo);
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet("GetOutgoes")]
        public IActionResult GetOutgoes()
        {
            return new JsonResult(db.Outgos.Include(o=>o.Item).Include(o=>o.User).Where(o=>o.User.UserName==User.Identity.Name || o.User.Family == Worker.GetFamilyByUserName(User.Identity.Name)).ToList());
        }
    }
}