    using System.ComponentModel.DataAnnotations;

    namespace MVC_Forum.Models
{
    public class User
    {
        public User(string name, bool isAdmin)
        {
            Name=name;
            IsAdmin=isAdmin;
            Friends = new List<User>();
        }
        public User(string name, bool isAdmin,DateTime created)
        {
            Name = name;
            IsAdmin = isAdmin;
            Created=created;
            Friends = new List<User>();
        }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Created { get; set; }
        public List<User> Friends { get; set; }
    }
}
