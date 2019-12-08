using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCostsProject.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/incomes")]
    public class IncomeController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public IncomeController(ApplicationContext context)
        {
            db = context;
            Worker = new DbWorker(db);
        }
        ///<summary>Add an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="200">Returns an income that was aded </response>
        ///<response code="403">if request data was incorrect</response>
        ///<response code="400">"Bad request"</response>
        //[Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Income income)
        {
            try
            {
                if (income != null)
                {
                    income.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    Worker.AddIncome(income);
                    Worker.MakeIncome(User.Identity.Name, income.Money);
                    return Ok(Worker.GetLastIncome());
                }
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }
        ///<summary>Delete an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200"></response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Income income = db.Incomes.FirstOrDefault(i => i.Id == id);

                if (income != null && income.User.UserName == User.Identity.Name)
                {
                    Worker.DeleteIncom(id);
                    return Ok();
                }
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
            }
        ///<summary>Edit an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns income that was edited</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpPatch]
        public IActionResult Patch(int id, string WorkType, DateTime Date)
        {
            try
            {
                Income income = db.Incomes.FirstOrDefault(i => i.Id == id);
                if (income != null && income.User.UserName == User.Identity.Name)
                {
                    Worker.EditIncome(id, WorkType, Date);
                    return Ok(income);
                }
                return Forbid();
            }
            catch
            {
                return BadRequest();
            }
        }
        ///<summary>Get an income or  incomes </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns income</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="404"> if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpGet]
        public IActionResult Get([FromHeader] int? id)
        {
            if (id != null)
            {
                try
                {
                    Income income = Worker.GetIncome(id);
                    if (income != null)
                    {
                        return Json(income);
                    }
                    return NotFound();
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                return new JsonResult(Worker.GetIncomes().Where(u => (u.User.UserName == User.Identity.Name || u.User.Family == Worker.GetFamilyByUserName(User.Identity.Name))));
            }
        }
    
    }

 }
