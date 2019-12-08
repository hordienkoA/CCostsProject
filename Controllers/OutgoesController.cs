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
    [Route("api/outgoes")]
    public class OutgoesController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public OutgoesController(ApplicationContext context)
        {
            db = context;
            Worker = new DbWorker(db);
        }

        ///<summary>Add an outgo</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns an outgo that was aded </response>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="403">if request data was incorrect</response>
        [HttpPost]
        public IActionResult Post([FromBody]Outgo outgo)
        {
            if (outgo != null)
            {
                outgo.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                Worker.AddOutgo(outgo);
                Worker.MakeOutgo(User.Identity.Name, outgo.Money);
                return Ok(Worker.GetLastOutgo());
            }
            return Forbid();
        }
        ///<summary>Edit an outgo</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns an outgo that was edited </response>
        ///<response code="403">if outho with that id not found</response>
        //[Authorize]
        [HttpPatch]
        public IActionResult Post(int id, double Money, DateTime Date)
        {
            Outgo outgo = db.Outgos.FirstOrDefault(o => o.Id == id);
            if (outgo != null && outgo.User.UserName == User.Identity.Name)
            {
                outgo.Money = Money;
                outgo.Date = Date;
                db.SaveChanges();
                return Ok(outgo);
            }
            return Forbid();
        }

        ///<summary>Delete an outgo</summary>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200"> </response>
        ///<response code="403">if outho with that id not found</response>
        //[Authorize]
        [HttpDelete]
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



        ///<summary>Get an outgo or outgoes</summary>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns outho</response>
        ///<response code="404"> if outgo with that id not found</response>
    
        //[Authorize]
        [HttpGet]
        public IActionResult GetOutgoes([FromHeader] int? id)
        {
            if (id != null)
            {

                Outgo outgo = Worker.GetOutgo(id);
                if (outgo != null)
                {
                    return Json(outgo);
                }
                return NotFound();
            }
            return new JsonResult(db.Outgos.Include(o => o.Item).Include(o => o.User).Where(o => o.User.UserName == User.Identity.Name).ToList());
        }
    }
}