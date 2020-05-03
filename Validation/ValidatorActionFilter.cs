using System;
using System.Collections.Generic;
using System.Linq;
using CCostsProject.json_structure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CCostsProject.Validation
{
    public class ValidatorActionFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.ModelState.IsValid)
            {
                 
                context.HttpContext.Response.StatusCode = 400;
                
                
                context.Result=new JsonResult(JsonConvert.DeserializeObject( JsonResponseFactory.CreateJson(null,new List<object>(context.ModelState.Keys),new List<string>(context.ModelState.Values.Select(v=>string.Join('\n',v.Errors.Select(t=>t.ErrorMessage)))))));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}