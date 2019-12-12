using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCostsProject.json_structure
{
    public class CustomStructureOfJsonRequest
    {
        public object data { get; set; }
        public List<CustomJsonObject> messages { get; set; }
        
        public CustomStructureOfJsonRequest()
        {
            messages = new List<CustomJsonObject>();
        }
    }
}
