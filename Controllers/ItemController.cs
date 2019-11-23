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
        /// <summary>
        /// This method adds item
        /// </summary>
        /// <remarks>
        /// POST /AddItem{
        /// "AvarageCost":1488,
        /// "Type":"test"
        /// }
        /// </remarks>
        /// <param name="item"></param>
        /// <returns></returns>
        
        [Authorize]
        [HttpPost("AddItem")]
        [Produces("application/json")]
        public IActionResult Post([FromBody]Item item)
        {
            if (item == null)
            {
                return BadRequest();
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

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Item item = db.Items.FirstOrDefault(i => i.Id == id);
            if (item==null)
            {
                return NotFound();
            }
            return Json(item);
        }

        [Authorize]
        [HttpDelete("DelItem")]
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
        [HttpPost("EditItem")]
        public IActionResult EditItem([FromBody]Item item)
        {
            if (item != null)
            {
                worker.EditItem(item.Id, item.AvarageCost, item.Type);
                return Ok();

            }
            return Forbid();
        }
    }
}