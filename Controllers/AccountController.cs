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
    public class AccountController : Controller
    {
        ApplicationContext db;
        IWorker Worker;
        public AccountController(ApplicationContext context, IInitializer init)
        {
            this.db = context;
            Worker = new UserWorker(db);
            init.CheckAndInitialize();

        }
        
        /// <summary>
        /// Authentication by JWT
        /// </summary>
        ///<response code="200">Returns an autherization token </response>
        ///<response code="401">Returns Invalid username or password or incorect request data</response>
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpPost("login")]
        public async System.Threading.Tasks.Task Token([FromBody] LoginViewModel value)
        {

            try
            {
                var identity = GetIdentity(value.username, value.password);
            if (identity == null)
            {
                Response.StatusCode = 401;
                await Response.WriteAsync(JsonResponseFactory.CreateJson("","Invalid username or password","Error",null));
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
            await Response.WriteAsync(JsonResponseFactory.CreateJson("","ok","Success",response));
            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error",null));
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
        ///<response code="200">If user was added successfull </response>
        ///<response code="401">if user with that username exist</response>
        ///<response code="400">Bad request</response>
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("registration")]
        public async System.Threading.Tasks.Task Post([FromBody]User user)
        {
            try
            {

                try
                {
                    Worker.AddEntity(user);
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok", "Success",
                        Worker.GetEntities().Cast<User>().LastOrDefault()));
                    return;
                }
                catch (NullReferenceException)
                {

                    Response.ContentType = "application/json";
                    Response.StatusCode = 400;
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("email or username",
                        "email or username are not unique", "Error", null));

                }


            }
            catch
            {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error",null));
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
        public async System.Threading.Tasks.Task Get([FromHeader] string id)
        {
            try
            {

                if (User.Identity.Name.Trim() == "Admin")
                {
                    if (Int32.TryParse(id,out var integerId))
                    {
                        User user = (User)Worker.GetEntity(integerId);
                        if (user != null)
                        {
                            Response.StatusCode = 200;
                            Response.ContentType = "application/json";
                            await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Ok","Success",user));
                            return;

                        }
                        Response.StatusCode = 404;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Not found", "Error",null));
                       

                    }

               
                    else if(id==null)
                    {
                        Response.StatusCode = 200;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("",  "Ok","Success", db.Users.Cast<ITable>().ToList()));
                        return;
                       
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.ContentType = "application/json";
                        await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error", null));
                    }
                }
                else
                {
                    Response.StatusCode = 403;
                    Response.ContentType = "application/json";
                    await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Permission denied", "Error",null));
                    return;
                }
            }
            catch
            {
               Response.StatusCode = 400;
               Response.ContentType = "application/json";
               await Response.WriteAsync(JsonResponseFactory.CreateJson("", "Bad request", "Error",null));
                
            }
        }
    }
}
