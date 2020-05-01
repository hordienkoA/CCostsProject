using System.Linq;
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
    [Route("api/goals")]
    public class GoalController:Controller
    {
        private ApplicationContext db;
        private IWorker goalWorker;
        private IWorker userWorker;

        public GoalController(ApplicationContext context)
        {
            db = context;
            goalWorker=new PlanWorker(db);
            userWorker=new UserWorker(db);
        }
        ///<summary> Get a goal or goals that current user owns</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Return all goals that was created by current user</response>
        ///<response code="401">If the user has not authorized</response>
        /// <response code="400">If the request date in incorrect</response>
        [HttpGet]
        //[Produces("application/json")]

        public async Task Get([FromHeader] int? id)
        {
            try
            {
                if (id != null)
                {
                    var goal = goalWorker.GetEntity(id) as Plan;
                    Response.ContentType = "application/json";
                    Response.StatusCode = goal != null && goal.User.UserName == User.Identity.Name ? 200 : 404;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(
                        Response.StatusCode == 200 ? goal : null));
                    return;
                }

                var goals = goalWorker.GetEntities().Cast<Plan>().Where(p => p.User.UserName == User.Identity.Name)
                    .ToList<object>();
                Response.ContentType = "application/json";
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(goals));
            }
            catch
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }
       
        ///<summary> Add a goal</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">if the goal was added </response>
        /// <response code="400">If the request data in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
       [HttpPost]
       public async Task Post([FromBody] Plan plan)
        {
            try
            {
                plan.UserId = userWorker.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == User.Identity.Name)
                    ?.Id;
                plan.Status = "Active";
                plan.CurrencyId = (plan.CurrencyId == 0||plan.CurrencyId==null)
                    ? ((User) userWorker.GetEntity(plan.UserId)).CurrencyId
                    : plan.CurrencyId;
                goalWorker.AddEntity(plan);
                Response.ContentType = "application/json";
                Response.StatusCode = 200;

                await Response.WriteAsync(JsonResponseFactory.CreateJson(goalWorker.GetEntities().LastOrDefault()));
            }
            catch
            {
                Response.ContentType = "application/json";
                Response.StatusCode = 400;
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }

        ///<summary> Delete a goal</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">if the goal was deleted </response>
        ///<response code="400">If the request data in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
        ///<response code="403">If current user doesn`t own this goal</response>
       [HttpDelete]
       public async Task Delete([FromHeader] string id)
       {
           try
           {
               var plan = goalWorker.GetEntity(int.Parse(id));
               if (plan == null)
               {
                   Response.StatusCode = 404;
                   Response.ContentType = "application/json";
                   await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                   return;
               }

               if ((plan as Plan)?.User.UserName != User.Identity.Name)
               {
                   Response.StatusCode = 403;
                   Response.ContentType = "application/json";
                   await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                   return;
               }

               Response.StatusCode = 200;
               Response.ContentType = "application/json";
               goalWorker.DeleteEntity(plan);
               await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
           }
           catch
           {
               Response.StatusCode = 400;
               Response.ContentType = "application/json";
               await Response.WriteAsync(JsonResponseFactory.CreateJson(  null));
           }
       }
        
       

    }
}