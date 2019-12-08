using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCostsProject.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/screen")]
    public class ScreenController : Controller
    {

        ApplicationContext db;
        DbWorker worker;
        public ScreenController(ApplicationContext context)
        {
            db = context;
            worker = new DbWorker(db);
        }

        ///<summary>Get incomes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all incomes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet("incomes-by-date")]
        public IActionResult GetIncomes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<Income> list = worker.GetIncomesByDateRange(fromDate, toDate);
                return Json(list.Where(i => i.User.UserName == User.Identity.Name).ToList());

            }
            catch
            {
                return BadRequest();
            }
        }

        ///<summary>Get outgoes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all outgoes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet("outgoes-by-date")]
        public IActionResult GetOutgoes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<Outgo> list = worker.GetOutgoesByDataRange(fromDate, toDate);
                return Json(list.Where(o => o.User.UserName == User.Identity.Name));

            }
            catch
            {
                return BadRequest();
            }
        }


        ///<summary>Get outgoes and incomes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all outgoes and incomes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet("outgoes-and-incomes-by-date")]
        public IActionResult GetOutgoesAndIncomes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<IMoneySpent> list = worker.GetOuthoesAndIncomesByDateRange(fromDate, toDate);
                return Json(list.Where(m => m.User.UserName == User.Identity.Name));

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}