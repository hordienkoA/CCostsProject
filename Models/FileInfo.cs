using System.ComponentModel.DataAnnotations;
using CConstsProject.Models;

namespace CCostsProject.Models
{
    public class FileInfo:ITable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
        
        
        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}