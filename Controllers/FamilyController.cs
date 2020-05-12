using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.Exceptions;
using CCostsProject.json_structure;
using CCostsProject.Models;
using CCostsProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CCostsProject.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/family")]
    public class FamilyController:Controller
    {
        
        private ApplicationContext db;
        private IWorker _worker;
        private IWorker UserWork;

        public FamilyController(ApplicationContext context)
        {
            db = context;
            _worker = new FamilyWorker(db);
            UserWork=new UserWorker(db);
        }
        
        ///<summary>Add a family</summary>
        /// <remarks>need "Authorization: Bearer jwt token" in the  header of request
        /// After adding , the current user will be owner of family  </remarks>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="200">Returns a family that was added</response>
        /// <response code="403">If family name is not unique</response>
        ///<response code="400">if request data was incorrect</response>
        [HttpPost]
        [Produces("application/json")]
        public async Task Post([FromBody] Family family)
        {
            try
            {
                

                var creator = UserWork.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == Response.HttpContext.User.Identity.Name);
                family.Users.Add(creator);
                
                
                _worker.AddEntity(family);

                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( 
                    _worker.GetEntities().LastOrDefault()));

            }
            catch (Exception ex )
            {
                Response.StatusCode = ex is FamilyAlreadyExistException?403:400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null,ex is FamilyAlreadyExistException?new List<object>{"Name"}:null,
                    ex is FamilyAlreadyExistException ? new List<string> { "family name is not unique" } : null));
            }

        }
        
        ///<summary> Get a family or families that current user owns</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Return all families that was created by current user</response>
        /// <response code="400">If the request date in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
        [HttpGet]
        [Produces("application/json")]
        public async Task Get()
        {
            try
            {
               
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( 
                        _worker.GetEntities().Cast<Family>()
                            .Where(f => f.Users.Exists(u=>u.UserName == HttpContext.User.Identity.Name)).ToList<object>()));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }

        }
        
        ///<summary> Add a user to a family</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">if the user was added to a family </response>
        /// <response code="400">If the request data in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
        /// <response code="403">If current user doesn`t own this family or the user witch may be added is already exist </response>
        [HttpPost("add-user")]
        [Produces("application/json")]
        public async Task AddUser([FromBody] AddUserToFamilyViewModel familyUserView)
        {
            try
            {
                /*if (familyUserView== null)
                {
                    Response.StatusCode = 400;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                    return;
                }*/

                
                if (!((Family)_worker.GetEntity(familyUserView.familyId)).Users.Contains(UserWork.GetEntity(familyUserView.userId))&&((Family)_worker.GetEntity(familyUserView.familyId)).Users.First().Id==
                    UserWork.GetEntities().Cast<User>().FirstOrDefault(u=>u.UserName==HttpContext.User.Identity.Name).Id)
                {
                    ((FamilyWorker) _worker).AddUser(familyUserView.familyId, familyUserView.userId);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }

                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                
            }
        }

        ///<summary> Get family users</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns all users that relate to appropriate family  </response>
        /// <response code="400">If the request date in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
        /// <response code="403">If the user is not a member of the family</response>
        /// <response code="404">If family with that id was not not found</response>
        [HttpGet("get-users")]
        [Produces("application/json")]
        public async Task GetUsers([FromHeader] string FamilyId)
        {
            try
            {
                

                var family = _worker.GetEntity(int.Parse(FamilyId));
                if (family == null)
                {
                    Response.StatusCode = 404;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( null)); 
                    return;
                }

                var users = ((Family) family).Users;
                if (!users.Contains(UserWork.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name)))
                {
                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null)); 
                    return;
                }
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(  users.Cast<ITable>().ToList())); 
                
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }

        /// <summary>
        /// Delete family
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">if family was deleted successful  </response>
        /// <response code="400">If the request date in incorrect</response>
        /// ///<response code="401">If the user has not authorized</response>
        /// /// <response code="403">If the current user is not owner of family with appropriate id</response>
        /// <response code="404">If we can`t find family with current id</response>
        /// <param name="FamilyId"></param>
        /// 
        /// <returns>teafd{t:3}</returns>
        [HttpDelete]
        public async Task DeleteFamily([FromHeader] string FamilyId)
        {
            try
            {
                var family = _worker.GetEntity(int.Parse(FamilyId));
                if (family == null)
                {
                    Response.StatusCode = 404;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                    return;

                }

                if (((Family) family).Users.First().Id ==
                    // ReSharper disable once PossibleNullReferenceException
                    UserWork.GetEntities().Cast<User>()
                        .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name).Id)
                {
                    _worker.DeleteEntity(family);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                    return;
                }

                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));



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