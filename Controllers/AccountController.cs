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

namespace CConstsProject.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        ApplicationContext db;
        DbWorker Worker;
        public AccountController(ApplicationContext context)
        {
            this.db = context;
            Worker = new DbWorker(db);
            if (!db.Users.Any())
            {
                List<User> users = new List<User>
                {
                    new User { UserName = "Admin", Password = "Admin",Position="Admin",WelcomeString="Hi, Master" }
                };
                db.Users.AddRange(users);
                db.SaveChanges();
            }

        }
        /// <summary>
        /// Authentication by JWT
        /// </summary>

        
        ///<response code="200">Returns an autherization token </response>
        ///<response code="400">Returns Invalid username or password</response>

        [HttpPost("/login")]
        public async System.Threading.Tasks.Task Token()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password");
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
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

        }
        /// <summary>
        /// Authentication by JWT
        /// </summary>
        ///<remarks>
        ///Sample request:
        ///
        ///Post /jsonLogin 
        ///{
        ///"username":"Admin",
        ///"password":"Admin"
        ///}
        /// </remarks>
        ///<response code="200">Returns an autherization token </response>
        ///<response code="400">Returns Invalid username or password</response>
        [Produces("application/json")]
        [HttpPost("/jsonLogin")]
        public async System.Threading.Tasks.Task Tocken([FromBody] LoginViewModel value)
        {

            
            var identity = GetIdentity(value.username, value.password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password");
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
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
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
        
        [HttpGet("GetUsers")]
        public IActionResult Get()
        {
            return new JsonResult(db.Users.ToList());
        }

        [HttpPost("AddUser")]
        public IActionResult Post([FromBody]User user)
        {
            if (user != null)
            {
                Worker.AddUser(user);
                return Ok();
            }
            return Forbid();
        }

      
        ///<response code="200">Returns the user </response>
        ///<response code="403">if auth username!=Admin</response>
        ///<response code="404">if user with those id not found </response>
        [Authorize]
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (User.Identity.Name == "Admin")
            {
                User user = Worker.GetUser(id);
                if (user != null)
                {
                    return Json(user);

                }


                return NotFound();

            }
            return Forbid("Permision denied");
        }
    }
}
