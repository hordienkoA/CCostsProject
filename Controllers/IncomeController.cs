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

        [Authorize]
        [HttpPost("AddIncome")]
        public IActionResult Post([FromBody] Income income)
        {
            if (income != null)
            {
                income.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                Worker.AddIncome(income);
                return Ok(income);
            }
            return Forbid();
        }
        [Authorize]
        [HttpDelete("DeleteIncome")]
        public IActionResult Delete(int id)
        {

            Income income = db.Incomes.FirstOrDefault(i => i.Id == id);
            
            if (income != null&&income.User.UserName==User.Identity.Name)
            {
                Worker.DeleteIncom(id);
                return Ok();
            }
            return Forbid();
        }
        [HttpPost("EditIncome")]
        public IActionResult Post(int id, string WorkType,DateTime Date)
        {
            Income income = db.Incomes.FirstOrDefault(i => i.Id == id);
            if (income != null&&income.User.UserName == User.Identity.Name)
            {
                Worker.EditIncome(id, WorkType,Date);
                return Ok(income);
            }
            return Forbid();
        }
        [Authorize]
        [HttpGet("GetIncomes")]
        public IActionResult Get()
        {
            return new JsonResult(Worker.GetIncomes().Where(u=>u.User.UserName==User.Identity.Name));
        }
    }

 }
