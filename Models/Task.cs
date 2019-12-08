using Newtonsoft.Json;

namespace CConstsProject.Models
{
    public class Task
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string True_rule { get; set; }
        public string False_rule{get;set;}
        public int? TaskManagerId { get; set; }
        public TaskManager TaskManager { get; set; }
    }
}