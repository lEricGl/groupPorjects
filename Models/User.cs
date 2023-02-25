using Microsoft.AspNetCore.Identity;

namespace assignment_one.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public int Role { get; set; }

        public User() { }
    }
}
