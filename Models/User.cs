using System.ComponentModel.DataAnnotations;


namespace GetSomeHelp.Models{
    public class User{
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string Location { get; set; }
        public string passHash { get; set; }
        public int stars { get; set; }
        public string profilePic { get; set; }
        public string Email { get; set; }

    }
}