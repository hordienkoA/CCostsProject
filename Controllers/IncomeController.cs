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
    [Route("api/[controller]")]
    public class IncomeController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public IncomeController(ApplicationContext context)
        {
            db = context;
            Worker = new DbWorker(db);
        }

        ///<response code="200">Returns an income that was aded </response>
        ///<response code="403">if request data was incorrect</response>
        [Authorize]
        [HttpPost("AddIncome")]
        public IActionResult Post([FromBody] Income income)
        {
            if (income != null)
            {
                income.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                Worker.AddIncome(income);
                Worker.MakeIncome(User.Identity.Name, income.Money);
                return Ok(income);
            }
            return Forbid();
        }
        ///<response code="200"></response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        [Authorize]
        [HttpDelete("DeleteIncome")]
        public IActionResult Delete(int id)
        {

            Income income = db.Incomes.FirstOrDefault(i => i.Id == id);

            if (income != null && income.User.UserName == User.Identity.Name)
            {
                Worker.DeleteIncom(id);
                return Ok();
            }
            return Forbid();
        }

        ///<response code="200">Returns income that was edited</response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        [HttpPost("EditIncome")]
        public IActionResult Post(int id, string WorkType, DateTime Date)
        {
            Income income = db.Incomes.FirstOrDefault(i => i.Id == id);
            if (income != null && income.User.UserName == User.Identity.Name)
            {
                Worker.EditIncome(id, WorkType, Date);
                return Ok(income);
            }
            return Forbid();
        }

        ///<response code="200">Returns income</response>
        ///<response code="404"> if income with that id not found</response>
        [Authorize]
        [HttpGet("GetIncome")]
        public IActionResult Get([FromBody]int id)
        {
            Income income = Worker.GetIncome(id);
            if (income != null)
            {
                return Json(income);
            }
            return NotFound();
        }
        [Authorize]

        [HttpGet("GetIncomes")]
        public IActionResult Get()
        {
            return new JsonResult(Worker.GetIncomes().Where(u=>(u.User.UserName==User.Identity.Name||u.User.Family==Worker.GetFamilyByUserName(User.Identity.Name))));
        }
    }

 }
