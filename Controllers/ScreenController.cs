using System;
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
        public async System.Threading.Tasks.Task GetIncomes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<Income> list = worker.GetIncomesByDateRange(fromDate, toDate);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(i => i.User.UserName == User.Identity.Name).ToList()));
                return;
                

            }
            catch
            {

                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }

        ///<summary>Get outgoes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all outgoes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet("outgoes-by-date")]
        public async System.Threading.Tasks.Task GetOutgoes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<Outgo> list = worker.GetOutgoesByDataRange(fromDate, toDate);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(o => o.User.UserName == User.Identity.Name)));
                return;
                

            }
            catch
            {

                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }


        ///<summary>Get outgoes and incomes by date range</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request. if we didn't specify a toDate, the current date will be selected </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns all outgoes and incomes in current date range </response>
        ///<response code="400">BadRequest</response>
        [HttpGet("outgoes-and-incomes-by-date")]
        public async System.Threading.Tasks.Task GetOutgoesAndIncomes([FromHeader]DateTime fromDate, [FromHeader]DateTime? toDate)
        {
            try
            {
                List<IMoneySpent> list = worker.GetOuthoesAndIncomesByDateRange(fromDate, toDate);
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", list.Where(m => m.User.UserName == User.Identity.Name)));
                return;

               

            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
    }
}