using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using Microsoft.AspNetCore.Authorization;
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
        }
        ///<summary>Add an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
       
        ///<response code="200">Returns an item that was aded </response>
        ///<response code="403">if request data was incorrect</response>

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Post([FromBody]Item item)
        {
            try
            {
                if (item == null)
                {
                    return Forbid();
                }
                worker.AddItem(item.AvarageCost, item.Type);
                return Ok(worker.GetLastItem());
            }
            catch
            {
                return BadRequest();
            }
        }

        ///<summary>Get an item or  items</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<returns code="200">return all items that was created by current user</returns>
        ///<response code="404"> if item with that id not found</response>
        [HttpGet("/api/items")]
        public IActionResult Get([FromHeader] int? id)
        {
            if (id == null)
            {
                return Json(worker.GetItems());
            }
            else
            {
                try
                {
                    Item item = db.Items.FirstOrDefault(i => i.Id == id);
                    if (item == null)
                    {
                        return NotFound();
                    }
                    return Json(item);
                }
                catch
                {
                    return BadRequest();
                }
            }
            
            
          }


        ///<summary>Delete an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200"></response>
        ///<response code="403"> if item with that id not found</response>

        [HttpDelete]
        public IActionResult DelItem(int id)
        {
            try
            {
                Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return Forbid();
            }
            worker.DeleteItem(id);
            return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        ///<summary>Edit an item</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="400">Bad request</response>
        ///<response code="200">Returns new item</response>
        ///<response code="403"> if item  not found</response>

        [HttpPatch]
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