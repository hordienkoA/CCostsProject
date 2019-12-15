using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.json_structure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async System.Threading.Tasks.Task Post([FromBody]Outgo outgo)
        {

            if (outgo != null)
            {
                outgo.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                Worker.AddOutgo(outgo);
                Worker.MakeOutgo(User.Identity.Name, outgo.Money);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", Worker.GetLastOutgo()));
                return;
                
            }
            Response.StatusCode = 403;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbbiden", "Error", null));
            return;
        }
        ///<summary>Edit an outgo</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns an outgo that was edited </response>
        ///<response code="403">if outho with that id not found</response>
        //[Authorize]
        [HttpPut]
        public async System.Threading.Tasks.Task Put([FromBody] Outgo outgo)
        {
            Outgo outg = db.Outgos.Include(o=>o.User).FirstOrDefault(o => o.Id == outgo.Id);
            if (outg != null && outg.User.UserName == User.Identity.Name)
            {
                Worker.EditOutgo(outgo);
                
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", outgo));
                return;
                
            }
            Response.StatusCode = 400;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            return;
        }

        ///<summary>Delete an outgo</summary>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200"> </response>
        ///<response code="403">if outho with that id not found</response>
        //[Authorize]
        [HttpDelete]
        public async System.Threading.Tasks.Task Delete([FromHeader]int id)
        {
            Outgo outgo = db.Outgos.Include(o => o.User).FirstOrDefault(o => o.Id == id);
            if (outgo != null && outgo.User.UserName == User.Identity.Name)
            {
                db.Outgos.Remove(outgo);
                db.SaveChanges();
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                return;
            }
            Response.StatusCode = 400;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            return;
        }



        ///<summary>Get an outgo or outgoes</summary>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns outho</response>
        ///<response code="404"> if outgo with that id not found</response>
    
        //[Authorize]
        [HttpGet]
        public async System.Threading.Tasks.Task GetOutgoes([FromHeader] string  id)
        {
            try
            {
                int IntegerId;
                if (Int32.TryParse(id, out IntegerId))
                {

                    Outgo outgo = Worker.GetOutgo(IntegerId);
                    if (outgo != null)
                    {
                        Response.StatusCode = 200;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", outgo));
                        return;

                    }
                    Response.StatusCode = 404;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Not found", "Error", null));
                    return;
                }
                else if (id == null)
                {
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", db.Outgos.Include(o => o.Item).Include(o => o.User).Where(o => o.User.UserName == User.Identity.Name).ToList()));
                    return;
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                }
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
          
        }
    }
}