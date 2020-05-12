using System.Collections.Generic;

namespace CCostsProject.json_structure
{
    public class JsonStructureExample<T>
    {
        public T data { get; set; }
        public List<CustomJsonObject> messages { get; set; }
    }
}