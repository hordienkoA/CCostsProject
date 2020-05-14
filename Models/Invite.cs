using System;

namespace CCostsProject.Models
{
    public class Invite:ITable
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string UserName{ get; set; }
        public DateTime Date { get; set; }
    }
}