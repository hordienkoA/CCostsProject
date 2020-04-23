﻿using Newtonsoft.Json;
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
                ? new { list = ((List<object>)data)}
                : data;
            var json = JsonConvert.SerializeObject(structureResponse);
            return json;
        }
    }
}
