using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCostsProject.Models;

namespace CCostsProject.json_structure
{
    public static class JsonResponseFactory
    {
        public static string  CreateJson(object data,List<object> fields=null,List<object> errors=null)
        {
            var structureResponse = new CustomStructureOfJsonRequest();
            if (fields != null)
            {
                foreach (var VARIABLE in fields)
                {
                    structureResponse.messages.Add(new CustomJsonObject
                        {field = VARIABLE, errors = string.Join("\n",errors[fields.IndexOf(VARIABLE)])});
                }
            }

            structureResponse.data = (data is IList)
                ? new { list = ((List<object>)data)}
                : data;
            var json = JsonConvert.SerializeObject(structureResponse);
            return json;
        }

        
    }
}
