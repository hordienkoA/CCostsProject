using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.json_structure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCostsProject.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/items")]
    public class ItemController : Controller
    {
        ApplicationContext db;
        DbWorker worker;
        public ItemController(ApplicationContext context)
        {
            db = context;
            worker = new DbWorker(db);
            if (!db.Items.Any())
            {
                db.Items.Add(new Item { Type = "Food", AvarageCost = 1488 });
                db.Items.Add(new Item { Type = "Games", AvarageCost = 228 });
                db.SaveChanges();
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
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbbiden", "Error", null));
                    return;
                }
                worker.AddItem(item.AvarageCost, item.Type);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", worker.GetLastItem()));
                return;
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

        ///<summary>Get an item or  items</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<returns code="200">return all items that was created by current user</returns>
        ///<response code="404"> if item with that id not found</response>
        [HttpGet("/api/items")]
        public async System.Threading.Tasks.Task Get([FromHeader] string id)
        {
            int IntegerId;
            if (id == null)
            {
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", worker.GetItems()));
                return;
              
            }
            else if(Int32.TryParse(id, out IntegerId))
            {
                try
                {
                    Item item = db.Items.FirstOrDefault(i => i.Id == IntegerId);
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
                    return;
                   
                }
                catch
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                }
            }
            else
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
        ///<response code="403"> if item with that id not found</response>

        [HttpDelete]
        public async System.Threading.Tasks.Task DelItem(int id)
        {
            try
            {
                Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbbiden", "Error", null));
                    return;
                }
            worker.DeleteItem(id);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                return;
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
        public IActionResult EditItem([FromBody]Item item)
        {
            try
            {
                if (item != null)
            {
                worker.EditItem(item.Id, item.AvarageCost, item.Type);
                return Ok(item);

            }
            return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}