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
    public class FamilyController : Controller
    {

        private ApplicationContext db;
        private IWorker _worker;
        private IWorker UserWork;
        private IWorker InviteWorker;

        public FamilyController(ApplicationContext context)
        {
            db = context;
            _worker = new FamilyWorker(db);
            UserWork = new UserWorker(db);
            InviteWorker = new InviteWorker(db);
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
                family.CreatedAt = DateTime.Now;


                _worker.AddEntity(family);

                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(
                    _worker.GetEntities().LastOrDefault()));

            }
            catch (Exception ex)
            {
                Response.StatusCode = ex is FamilyAlreadyExistException ? 403 : 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null,
                    ex is FamilyAlreadyExistException ? new List<object> {"Name"} : null,
                    ex is FamilyAlreadyExistException ? new List<string> {"family name is not unique"} : null));
            }

        }

        ///<summary> Get a family  that current user owns</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns a family this user is in </response>
        /// <response code="400">If the request date in incorrect</response>
        ///<response code="401">If the user has not authorized</response>
        [ProducesResponseType(typeof(JsonStructureExample<FamilyJson>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]
        [HttpGet]
        [Produces("application/json")]
        public async Task Get()
        {
            try
            {
                var currentUser = UserWork.GetEntities().Cast<User>().First(u => u.UserName == User.Identity.Name);
                if (currentUser.Family == null)
                {
                    Response.StatusCode = 404;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }
              
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(
                    _worker.GetEntities().Cast<Family>()
                        .Where(f => f.Users.Exists(u => u.UserName == HttpContext.User.Identity.Name)).Select(
                            o => new
                            {
                                Id = o.Id,
                                Name=o.Name,
                                Creator = o.Users.First()?.UserName, 
                                CreatedAt = o.CreatedAt,
                                members = o.Users.Select(
                                        u => new
                                        {
                                            Id = u.Id,
                                            role = u.UserName == o.Users.First()?.UserName ? "Owner" : "Member",
                                            Nickname = u.UserName, u.FirstName, u.SecondName, u.Email,Money=u.Transactions.Where(t=>t.Type.ToString()==TransactionType.Family.ToString()).Sum(t=>t.Money)
                                        })
                            }).First()));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }

        }

        /// <summary>Add an invite to a family </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns an invite that was added </response>
        /// <response code="400">If the request date in incorrect</response>
        ///<response code="403">If the user has not a family or if he is not an owner </response>
        ///<response code="404">If the user with current username not found </response>
        


        [ProducesResponseType(typeof(JsonStructureExample<Invite>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 403)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 404)]
        
        [HttpPost("invite")]
        public async Task AddInvite([FromHeader] string userName)
        {
            try
            {

                var user = UserWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == userName);
                var currentUser = UserWork.GetEntities().Cast<User>()
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user != null)
                {
                    if (currentUser.Family != null)
                    {
                        if (currentUser?.Family.Users.FirstOrDefault()?.UserName == User.Identity.Name)
                        {
                            InviteWorker.AddEntity(new Invite
                                {Date = DateTime.Now, FamilyId = (int) currentUser.FamilyId, UserName = userName});
                            Response.StatusCode = 200;
                            Response.ContentType = "application/json";
                            await Response.WriteAsync(
                                JsonResponseFactory.CreateJson(InviteWorker.GetEntities().LastOrDefault()));
                            return;
                        }

                        Response.StatusCode = 403;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson(null,
                            new List<object> {""}, new List<string> {"Current user is not owner of current family"}));
                        return;
                    }

                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null, new List<object> {""},
                        new List<string> {"Current user is not in family now"}));
                    return;
                }

                Response.StatusCode = 404;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null, new List<object> {"username"},
                    new List<string> {"User with current username not found"}));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }


        ///<summary>Gets all invitation of current user </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns all invitations of current user </response>
        ///<response code="400">If the request date in incorrect</response>

        [ProducesResponseType(typeof(JsonStructureExample<List<Invite>>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]

        [HttpGet("invitations")]
        public async Task GetInvitations()
        {
            try
            {
                Response.StatusCode = 200;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(InviteWorker.GetEntities().Cast<Invite>()
                    .Where(i => i.UserName == User.Identity.Name).ToList<object>()));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }

        ///<summary>Accept the invite </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">If the user was added to a family </response>
        ///<response code="400">If the request date in incorrect</response>        
        ///<response code="404">If the invite with current id was not found</response>
        [ProducesResponseType(typeof(JsonStructureExample<object>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 404)]


        [HttpPost("invitations/accept")]
        public async Task AcceptInvite([FromHeader] int id)
        {
            try
            {
                var invite = InviteWorker.GetEntities().Cast<Invite>()
                    .FirstOrDefault(i => i.UserName == User.Identity.Name && i.Id == id);
                if (invite != null)
                {
                    ((FamilyWorker) _worker).AddUser(invite.FamilyId,
                        UserWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id);
                    InviteWorker.DeleteEntity(invite);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }

                Response.StatusCode = 404;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null, new List<object> {""},
                    new List<string> {"The invite with current id was not found"}));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }

        ///<summary>Leave the family </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">If the user was successfully deleted from family</response>
        ///<response code="400">If the request date in incorrect</response>        

        [ProducesResponseType(typeof(JsonStructureExample<object>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]

        [HttpDelete("leave")]
        public async Task LeaveFamily()
        {
            try
            {
                var user = UserWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name);
                user.Family = null;
                UserWork.EditEntity(user);
                Response.StatusCode = 200;
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

        ///<summary>Cancel the invite</summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">If the invite was deleted </response>
        ///<response code="400">If the request date in incorrect</response>        
        ///<response code="404">If the invite with current id was not found</response>
        [ProducesResponseType(typeof(JsonStructureExample<object>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 404)]
        [HttpDelete("invitations/cancel")]
        public async Task CancelInvite([FromHeader] int id)
        {
            try
            {

                var invite = InviteWorker.GetEntities().Cast<Invite>()
                    .FirstOrDefault(i => i.UserName == User.Identity.Name && i.Id == id);
                if (invite != null)
                {

                    InviteWorker.DeleteEntity(invite);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }

                Response.StatusCode = 404;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null, new List<object> {""},
                    new List<string> {"The invite with current id was not found"}));
            }
            catch
            {

                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }

        /// <summary>Kick a user </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">If the user was successfully kicked</response>
        ///<response code="403">If the user has not permissions to perform this operation</response>
        ///<response code="400">If the request date in incorrect</response>
        [ProducesResponseType(typeof(JsonStructureExample<object>), 200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 400)]
        [ProducesResponseType(typeof(JsonStructureExample<object>), 403)]
        
        [HttpDelete("kick")]
        public async Task KickUser([FromHeader] int id)
        {
            try
            {
                var user = UserWork.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user.Family != null&&user.Family.Users.FirstOrDefault()?.UserName==User.Identity.Name)
                {
                    var family = user.Family;
                    family.Users.Remove((User) UserWork.GetEntity(id));
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                    return;
                }

                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null,new List<object>{""},new List<string>{"The user has not permissions to perform this operation"}));


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
        public async Task DeleteFamily()
        {
            try
            {
                var family = UserWork.GetEntities().Cast<User>().First(u=>u.UserName==User.Identity.Name).Family;
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