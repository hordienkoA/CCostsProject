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
    [Route("api/currencies")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CurrencyController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public CurrencyController(ApplicationContext context)
        {
            db = context;
            Worker = new DbWorker(db);
        }
        /// <summary>
        /// Add a currency
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        /// <response code="200">Return a currency </response>
        /// response code= "401">if the user has not authorized</response>
        ///<response code="400">"Bad request"</response>
        /// <param name="currency"></param>
        /// <returns></returns>

        [HttpPost]
        public async System.Threading.Tasks.Task Post([FromBody] Currency currency)
        {
            try
            {
                if (currency != null)
                {
                    Worker.AddCurrency(currency);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", Worker.GetLastCurrency()));
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
        /// <summary>
        /// Edit a currency
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns income that was edited</response>
        ///response code= "401">if the user has not authorized</response>
        ///<response code="400">"Bad request"</response>
        [HttpPatch]
        public async System.Threading.Tasks.Task Patch(int id,string name)
        {
            try
            {
                Currency cur = null;
                if (Worker.EditCurrency(id, name, out cur))
                {

                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", cur));
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
                return;
            }
        }
        /// <summary>
        /// Delete a currency
        /// </summary>
        ///<response code="200"></response>
        ///<response code="400">"Bad request"</response>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>

        [HttpDelete]
        public async System.Threading.Tasks.Task Delete(int id)
        {
            try
            {
                if (Worker.DeleteCurrency(id))
                {
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                    return;
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "This currensy are using right now", "Error", null));
                }
            }
            catch
            {

                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }


        /// <summary>
        /// Get a currency or currencies
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns currency or currencies</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="404"> if currency with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpGet]
        public async System.Threading.Tasks.Task Get(string id)
        {
            try
            {


                int IntegerId;
                if (Int32.TryParse(id, out IntegerId))
                {
                    Currency currency = Worker.GetCurrency(IntegerId);
                    if (currency != null)
                    {
                        Response.StatusCode = 200;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", currency));
                        return;
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Not found", "Error", null));
                        return;
                    }
                }
                else if (id == null)
                {
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", Worker.GetCurrencies()));
                    return;
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                }
            }
            catch {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
    }
}