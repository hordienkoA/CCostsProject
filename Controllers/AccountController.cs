using CConstsProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using CCostsProject;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CCostsProject.Models;
using RestSharp;
using CCostsProject.json_structure;
using System.Text;
using System.Net.Http;

namespace CConstsProject.Controllers
{
    [Route("api/accounts")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        ApplicationContext db;
        IWorker Worker;
        IWorker FamilyWork;
        public AccountController(ApplicationContext context, IInitializer init)
        {
            db = context;
            Worker = new UserWorker(db);
            FamilyWork=new FamilyWorker(db);
            init.CheckAndInitialize();

        }
        
        /// <summary>
        /// Authentication by JWT
        /// </summary>
        ///<response code="200">Returns an authorization token </response>
        ///<response code="400">If the request data was incorrenct</response>
        ///<response code="401">Returns Invalid username or password or incorrect request data</response>
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JsonStructureExample<LoginJsonExample>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [HttpPost("login")]
        public async Task Token([FromBody] LoginViewModel value)
        {

            try
            {
                var identity = GetIdentity(value.username, value.password);
            if (identity == null)
            {
                Response.StatusCode = 401;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null, new List<object> { "Username | Password" }, new List<string> { "Invalid username or password" }));
                    return;
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson(response));
            }
            catch(Exception ex )
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null,null));
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private ClaimsIdentity GetIdentity(StringValues username, StringValues password)
        {
            User user = db.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType,user.UserName),

                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

       
        /// <summary>
        /// Add new user
        /// </summary>
        ///<response code="200">If user was added successful </response>
        ///<response code="401">if user with that username exist</response>
        ///<response code="400">Bad request</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(JsonStructureExample<User>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]

        [Produces("application/json")]
        [HttpPost("registration")]
        public async Task Post([FromBody]User user)
        {
            try
            {

                try
                {
                    Worker.AddEntity(user);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( 
                        Worker.GetEntities().Cast<User>().LastOrDefault()));
                    return;
                }
                catch (NullReferenceException)
                {

                    Response.ContentType = "application/json";
                    Response.StatusCode = 400;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(null,
                          new List<object> { "Email | UserName" }, new List<string> { "email or username are not unique" }));

                }


            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            }
        }


        /// <summary>
        /// Get a user by id or users 
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns the user </response>
        ///<response code="403">if auth username!=Admin</response>
        ///<response code="401">if the user has not authorized</response>
        ///<response code="404">if user with that id not found </response>
        
        [HttpGet]
        public async Task Get([FromHeader] string id)
        {
            try
            {

                
                    if (Int32.TryParse(id,out var integerId))
                    {
                        User user = (User)Worker.GetEntity(integerId);
                        if (user != null)
                        {
                            Response.StatusCode = 200;
                            Response.ContentType = "application/json";
                            await Response.WriteAsync(JsonResponseFactory.CreateJson(new {Id=user.Id,UserName=user.UserName}));
                            return;

                        }
                        Response.StatusCode = 404;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                       

                    }

               
                    else if(id==null)
                    {
                        Response.StatusCode = 200;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson(db.Users.Select(u=>new {Id=u.Id,UserName=u.UserName}).ToList<object>()));
                        return;
                       
                    }
                
                else
                {
                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                    return;
                }
            }
            catch
            {
               Response.StatusCode = 400;
               Response.ContentType = "application/json";
               await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
                
            }
        }
        
        [HttpPatch("leave-family")]
        public async Task LeaveFamily()
        {
            
            var user = Worker.GetEntities().Cast<User>().FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user.Family == null)
            {
                Response.StatusCode = 403;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
                return;
            }

            var family = user.FamilyId;
            user.FamilyId = null;
            
            Worker.EditEntity(user);
            
            if (((Family) FamilyWork.GetEntity(family)).Users.Count == 0)
            {
                FamilyWork.DeleteEntity(FamilyWork.GetEntity(family));
            }
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonResponseFactory.CreateJson(null));
            
            

        }
    }
}
