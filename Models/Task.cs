using System.ComponentModel.DataAnnotations;

namespace GetSomeHelp.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public string Asker { get; set; }
        public string Accepter { get; set; }
        public bool Finished { get; set; }
        
    }
}