using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.json_structure;
using CCostsProject.Models;
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
        private IWorker OutgoWork;
        private IWorker UserWork;
        private ITransactionManager _manager;
        public OutgoesController(ApplicationContext context,ITransactionManager manager)
        {
            db = context;
            OutgoWork = new OutgoWorker(db);
            UserWork=new UserWorker(db);
            _manager = manager;
        }

        ///<summary>Add an outgo</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns an outgo that was added </response>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="403">if request data was incorrect</response>
        /// 
        [HttpPost]
        public async System.Threading.Tasks.Task Post([FromBody]Outgo outgo)
        {

            if (outgo != null)
            {
                outgo.User = UserWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name);
                OutgoWork.AddEntity(outgo);
                _manager.undo(User.Identity.Name, outgo.Money);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", OutgoWork.GetEntities().LastOrDefault()));
                return;
                
            }
            Response.StatusCode = 403;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbidden", "Error", null));
            return;
        }
        ///<summary>Edit an outgo</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns an outgo that was edited </response>
        ///<response code="403">if outgo with that id not found</response>
        //[Authorize]
        [HttpPut]
        public async System.Threading.Tasks.Task Put([FromBody] Outgo outgo)
        {
            Outgo outg = OutgoWork.GetEntities().Cast<Outgo>().FirstOrDefault(o => o.Id == outgo.Id);
            if (outg != null && outg.User.UserName == User.Identity.Name)
            {
                OutgoWork.EditEntity(outgo);
                
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
        ///<response code="403">if outgo with that id not found</response>
        //[Authorize]
        [HttpDelete]
        public async System.Threading.Tasks.Task Delete([FromHeader]int id)
        {
            Outgo outgo = OutgoWork.GetEntities().Cast<Outgo>().FirstOrDefault(o => o.Id == id);
            if (outgo != null && outgo.User.UserName == User.Identity.Name)
            {
                OutgoWork.DeleteEntity(outgo);
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
                if (Int32.TryParse(id, out var integerId))
                {

                    Outgo outgo = (Outgo)OutgoWork.GetEntity(integerId);
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

                if (id == null)
                {
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", OutgoWork.GetEntities().Cast<Outgo>().Where(o => o.User.UserName == User.Identity.Name).Cast<ITable>().ToList()));
                    return;
                }

                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
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