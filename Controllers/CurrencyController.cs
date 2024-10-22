﻿using System;
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
using Microsoft.EntityFrameworkCore.Internal;

namespace CCostsProject.Controllers
{
    [Route("api/currencies")]
    
    public class CurrencyController : Controller
    {
        ApplicationContext db;
        IWorker Worker;
        public CurrencyController(ApplicationContext context)
        {
            db = context;
            Worker = new CurrencyWorker(db);
            //init.CheckAndInitialize();
        }
       

        /// <summary>
        /// Get a currency or currencies
        /// </summary>
        ///<remarks>need "Authorization: Bearer jwt token" in the  header of request</remarks>
        ///<response code="200">Returns currency or currencies</response>
        ///<response code= "401">if the user has not authorized</response>
        ///<response code="400">"Bad request"</response>
        [ProducesResponseType(typeof(JsonStructureExample<List<CurrencyJsonExample>>),200)]
        [ProducesResponseType(typeof(JsonStructureExample<object>),400)]
        [HttpGet]
        public async Task Get()
        {
            try
            {
                
                    Response.StatusCode = 200;
                    Response.ContentType = "application/json";
                    
                    await Response.WriteAsync(JsonResponseFactory.CreateJson(Worker.GetEntities().Cast<Currency>().Select(c => new {Id = c.Id, Name = c.cc}).ToList<object>()));
                
                
            }
            catch {
                Response.StatusCode = 400;
                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonResponseFactory.CreateJson( null));
            }
        }
    }
}