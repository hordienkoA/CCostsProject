using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CConstsProject.Models
{
    public class TaskManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public List<Models.Task> Tasks { get; set; }
        public TaskManager()
        {
            Tasks = new List<Task>();
        }
    }
}
