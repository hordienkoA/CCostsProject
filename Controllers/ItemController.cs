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

namespace CCostsProject.Controllers
{
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/items")]
    public class ItemController : Controller
    {
        ApplicationContext db;
        IWorker worker;
         IWorker userWork;
        public ItemController(ApplicationContext context,IInitializer init)
        {
            db = context;
            worker = new ItemWorker(db);
            userWork=new UserWorker(db);
            init.CheckAndInitialize();
        }
        
         ///<summary>Get an item or  items</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<returns code="200">return all items that was created by current user</returns>
        ///<response code="404"> if item with that id not found</response>
        [HttpGet("/api/items")]
        public async System.Threading.Tasks.Task Get([FromHeader] string id)
        {
            try
            {
            if (id == null)
            {
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", worker.GetEntities().Cast<Item>().Where(i=>i.User.UserName==User.Identity.Name)));
                return;
              
            }

            if(Int32.TryParse(id, out var integerId))
            {
               
                    Item item = worker.GetEntities().Cast<Item>().FirstOrDefault(i => i.Id == integerId&&i.User.UserName==User.Identity.Name);
                    if (item == null)
                    {
                        Response.StatusCode = 404;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Not found", "Error", null));
                        return;
                    }
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", item));
                    
            }
           
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }


        }

        
        ///<summary>Add an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns an item that was aded </response>
        ///<response code="403">if request data was incorrect</response>

        [HttpPost]
        [Produces("application/json")]
        public async System.Threading.Tasks.Task Post([FromBody]Item item)
        {
            try
            {
                if (item == null)
                {
                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbidden", "Error", null));
                    return;
                }

                item.User = userWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (item.CurrencyId == 0) item.CurrencyId = item.User.CurrencyId;
                worker.AddEntity(item);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", worker.GetEntities().LastOrDefault()));
                return;
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

       

        ///<summary>Delete an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200"></response>
        ///<response code="403"> if item with that id not found or the current user has not permission</response>

        [HttpDelete]
        public async System.Threading.Tasks.Task DelItem([FromHeader]int id)
        {
            try
            {
                Item item = worker.GetEntities().Cast<Item>().FirstOrDefault(i => i.Id == id);
            if (item != null&&item.User.UserName == User.Identity.Name)
            {
                worker.DeleteEntity(item);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                return;
            }
            Response.ContentType = "application/json";

            Response.StatusCode = 403;
            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbidden", "Error", null));
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

        ///<summary>Edit an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200">Returns new item</response>
        ///<response code="403"> if item  not found</response>

        [HttpPut]
        public async Task EditItem([FromBody] Item item)
        {
            try
            {
                if (item != null && item.User.UserName == User.Identity.Name)
                {
                    worker.EditEntity(item);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", item));
                }
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));            }
        }
    }
}