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
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        ApplicationContext db;
        DbWorker worker;
        public ItemController(ApplicationContext context)
        {
            db = context;
            worker = new DbWorker(db);
        }

        ///<response code="200">Returns an item that was aded </response>
        ///<response code="403">if request data was incorrect</response>

        [Authorize]
        [HttpPost("AddItem")]
        [Produces("application/json")]
        public IActionResult Post([FromBody]Item item)
        {
            if (item == null)
            {
                return Forbid();
            }
            worker.AddItem(item.AvarageCost, item.Type);
            return Ok(item); 
        }

        [Authorize]
        [HttpGet("GetItems")]
        public IActionResult Get()
        {
            return Json(worker.GetItems());
        }

        ///<response code="200">Returns item</response>
        ///<response code="404"> if item with that id not found</response>
        [Authorize]
        [HttpGet("GetItem")]
        public IActionResult Get([FromBody]int id)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item==null)
            {
                return NotFound();
            }
            return Json(item);
        }

        ///<response code="200"></response>
        ///<response code="403"> if item with that id not found</response>
        [Authorize]
        [HttpDelete("DeleteItem")]
        public IActionResult DelItem(int id)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return Forbid();
            }
            worker.DeleteItem(id);
            return Ok();
        }

        ///<response code="200">Returns new item</response>
        ///<response code="403"> if item  not found</response>
        [HttpPost("EditItem")]
        public IActionResult EditItem([FromBody]Item item)
        {
            if (item != null)
            {
                worker.EditItem(item.Id, item.AvarageCost, item.Type);
                return Ok(item);

            }
            return Forbid();
        }
    }
}