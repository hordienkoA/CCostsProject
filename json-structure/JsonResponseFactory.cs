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
        public static string  CreateJson(string field,object text,string type,object data)
        {
            var structureResponse = new CustomStructureOfJsonRequest();
            structureResponse.messages.Add(new CustomJsonObject { field = field, text = text, type = type });
            structureResponse.data = (data is IList)
                ? new {count = ((List<ITable>) data).Count, list = ((List<ITable>)data).Cast<object>().ToList()}
                : new {count = (data==null)?0:1, list=new List<object>(){data}};
            var json = JsonConvert.SerializeObject(structureResponse);
            return json;
        }
    }
}
