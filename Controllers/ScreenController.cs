/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Route("api/points")]
    public class ScreenController : Controller
    {

        ApplicationContext db;
        private IWorker IncomeWork;
        private IWorker OutgoWork;
        public ScreenController(ApplicationContext context)
        {
            db = context;
            IncomeWork = new IncomeWorker(db);
            OutgoWork=new OutgoWorker(db);
        }

        /////<summary>Get incomes by date range</summary>
        /////<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        /////<response code="401">if the user has not authorized</response>
        /////<response code="200">Returns all incomes in current date range </response>
        /////<response code="400">BadRequest</response>
        //[HttpGet("incomes-by-date")]
        //public async System.Threading.Tasks.Task GetIncomes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        //{
        //    try
        //    {
        //        List<Income> list = worker.GetIncomesByDateRange(fromDate, toDate);
        //        Response.StatusCode = 200;
        //        Response.ContentType = "application/json";
        //        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(i => i.User.UserName == User.Identity.Name).ToList()));
        //        return;


        //    }
        //    catch
        //    {

        //        Response.StatusCode = 400;
        //        Response.ContentType = "application/json";
        //        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
        //    }
        //}

        /////<summary>Get outgoes by date range</summary>
        /////<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        /////<response code="401">if the user has not authorized</response>
        /////<response code="200">Returns all outgoes in current date range </response>
        /////<response code="400">BadRequest</response>
        //[HttpGet("outgoes-by-date")]
        //public async System.Threading.Tasks.Task GetOutgoes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        //{
        //    try
        //    {
        //        List<Outgo> list = worker.GetOutgoesByDataRange(fromDate, toDate);
        //        Response.StatusCode = 200;
        //        Response.ContentType = "application/json";
        //        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(o => o.User.UserName == User.Identity.Name)));
        //        return;


        //    }
        //    catch
        //    {

        //        Response.StatusCode = 400;
        //        Response.ContentType = "application/json";
        //        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
        //    }
        //}


        ///<summary>Get outgoes and incomes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected<br/>
        ///"type": string (incomes / outgoes / items / incomesAndOutgoes)<br/>
        ///"period":string (lastMonth / lastThreeMonth / lastHalfYear / lastYear / " " blank - all time)</remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all outgoes and incomes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet]
        public async System.Threading.Tasks.Task Get([FromHeader]string type, [FromHeader]string  period)
        {
           
            try
            {
                string trimedPeriod = period != null ? period.Trim() : "";
                string trimedType = type != null ? type.Trim() : "";
                if (trimedType == "incomes")
                {
                    List<Income> list = worker.GetIncomesByDateRange(trimedPeriod == "lastMonth" ? DateTime.Today.AddMonths(-1) :
                        trimedPeriod == "lastThreeMonth" ? DateTime.Today.AddMonths(-3) : trimedPeriod == "lastHalfYear" ?
                        DateTime.Today.AddMonths(-6) : trimedPeriod == "lastYear" ? DateTime.Today.AddYears(-1) : DateTime.Today.AddYears(-60), DateTime.Now);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(i => i.User.UserName == User.Identity.Name).ToList()));
                    return;
                }
                else if (trimedType == "outgoes")
                {
                    List<Outgo> list = worker.GetOutgoesByDataRange(trimedPeriod == "lastMonth" ? DateTime.Today.AddMonths(-1) :
                        trimedPeriod == "lastThreeMonth" ? DateTime.Today.AddMonths(-3) : trimedPeriod == "lastHalfYear" ?
                        DateTime.Today.AddMonths(-6) : trimedPeriod == "lastYear" ? DateTime.Today.AddYears(-1) : DateTime.Today.AddYears(-60), DateTime.Now);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(o => o.User.UserName == User.Identity.Name)));
                    return;
                }
                else if(trimedType== "incomesAndOutgoes")
                {
                    List<IMoneySpent> list = worker.GetOuthoesAndIncomesByDateRange(trimedPeriod == "lastMonth" ? DateTime.Today.AddMonths(-1) :
                        trimedPeriod == "lastThreeMonth" ? DateTime.Today.AddMonths(-3) : trimedPeriod == "lastHalfYear" ?
                        DateTime.Today.AddMonths(-6) : trimedPeriod == "lastYear" ? DateTime.Today.AddYears(-1) : DateTime.Today.AddYears(-60), DateTime.Now);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(m => m.User.UserName == User.Identity.Name)));
                    return;
                }
                else if (trimedType == "item")
                {
                    List<Item> list = worker.GetItems();
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list));
                    return;
                }
                else if(trimedType=="")
                {

                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("type", "Bad request", "Error",null));
                    return;
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                    return;
                }
               

            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
    }
}*/