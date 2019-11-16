using System;

namespace CConstsProject.Models
{
    public  class Outgo
    {
        public int Id { get; set; }
        public double Money { get; set; }
        public DateTime Date { get; set; }
        public int? ItemId { get; set; }
        public Item Item { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

    }
}