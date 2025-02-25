﻿using System;
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
        ///<response code="400">If the response body is incorrect</response>
        [ProducesResponseType(typeof(JsonStructureExample<List<Item>>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),404)]


        [HttpGet("/api/items")]
        public async System.Threading.Tasks.Task Get([FromHeader] string id)
        {
            try
            {
                if (id == null)
                {
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(  worker.GetEntities().Cast<Item>().Where(i=>i.User.UserName==User.Identity.Name).ToList<object>()));
                    return;
              
                }

                if(Int32.TryParse(id, out var integerId))
                {
               
                    Item item = worker.GetEntities().Cast<Item>().FirstOrDefault(i => i.Id == integerId&&i.User.UserName==User.Identity.Name);
                    if (item == null)
                    {
                        Response.StatusCode = 404;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                        return;
                    }
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( item));
                    
                }
           
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }


        }

        
        ///<summary>Add an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns an item that was added </response>
        ///<response code="400">If the response body is incorrect</response>
        ///<response code="403">if request data was incorrect</response>

        [ProducesResponseType(typeof(JsonStructureExample<Item>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),403)]
        
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
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }

                item.User = userWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (item.CurrencyId == null||item.CurrencyId==0) item.CurrencyId = item.User.CurrencyId;
                worker.AddEntity(item);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(worker.GetEntities().LastOrDefault()));
                return;
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }
        }

       

        ///<summary>Delete an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200"></response>
        ///<response code="403"> if item with that id not found or the current user has not permission</response>

        [ProducesResponseType(typeof(JsonStructureExample<object>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),403)]
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
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                return;
            }
            Response.ContentType = "application/json";

            Response.StatusCode = 403;
            await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
            }
        }

        ///<summary>Edit an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200">Returns new item</response>

        [ProducesResponseType(typeof(JsonStructureExample<Item>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]

        

        [HttpPut]
        public async Task EditItem([FromBody] Item item)
        {
            try
            {
                var current_item = (Item) worker.GetEntity(item.Id);
                if (current_item != null && current_item.User.UserName == User.Identity.Name)
                {
                    worker.EditEntity(item);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(  item));
                }
            }
            catch
            {
                Response.ContentType = "application/json";

                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));            }
        }
    }
}