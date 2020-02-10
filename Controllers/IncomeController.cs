using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.json_structure;
using CCostsProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CCostsProject.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/incomes")]
    public class IncomeController : Controller
    {
        ApplicationContext db;
        IWorker Worker;
        private ITransactionManager _manager;
        public IncomeController(ApplicationContext context,ITransactionManager manager)
        {
            db = context;
            _manager = manager;
            Worker = new IncomeWorker(db);
        }
        ///<summary>Add an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="200">Returns an income that was aded </response>
        ///<response code="403">if request data was incorrect</response>
        ///<response code="400">"Bad request"</response>
        //[Authorize]
        [HttpPost]
        public async System.Threading.Tasks.Task Post([FromBody] Income income)
        {
            try
            {
                if (income != null)
                {
                    income.User = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    Worker.AddEntity(income);
                    _manager.execute(User.Identity.Name, income.Money);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", Worker.GetEntities().Cast<Income>().LastOrDefault()));
                    return;
                }
                Response.StatusCode = 400;
                Response.ContentType= "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                return ;
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
               
            }
        }
        ///<summary>Delete an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200"></response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpDelete]
        public async System.Threading.Tasks.Task Delete([FromHeader]int id)
        {
            try
            {
                Income income = db.Incomes.Include(i=>i.User).FirstOrDefault(i => i.Id == id);

                if (income != null && income.User.UserName == User.Identity.Name)
                {
                    Worker.DeleteEntity(income);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", null));
                    return;
                }
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                return;
            }
            catch
            {
                Response.StatusCode = 400;
               Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
            }
        ///<summary>Edit an income</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns income that was edited</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="403">If user has not permission for this operation or if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpPut]
        public async System.Threading.Tasks.Task Put([FromBody] Income income)
        {
            try
            {
                Income inc = db.Incomes.Include(i=>i.User).FirstOrDefault(i => i.Id == income.Id);
                if (inc != null && inc.User.UserName == User.Identity.Name)
                {
                    Worker.EditEntity(income);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", income));
                    return;
                }
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Forbbiden", "Error", null));
                return;
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
        ///<summary>Get an income or  incomes </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns income</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="404"> if income with that id not found</response>
        ///<response code="400">"Bad request"</response>
        [HttpGet]
        public async System.Threading.Tasks.Task Get([FromHeader] string id)
        {
            int IntegerId;
            if (Int32.TryParse(id, out IntegerId))
            {
                try
                {
                    Income income = (Income)Worker.GetEntity(IntegerId);
                    if (income != null)
                    {
                        Response.StatusCode = 200;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", income));
                        return;
                       
                    }
                    Response.StatusCode = 404;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Not found", "Error", null));
                    return;
                }
                catch
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                }
            }
            else if (id == null)
            {
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success", Worker.GetEntities().Cast<Income>().Where(u => (u.User.UserName == User.Identity.Name /*|| u.User.Family == Worker.GetFamilyByUserName(User.Identity.Name)*/))));
                return;
                
            }

            else
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
            }
        }
    
    }

 }
