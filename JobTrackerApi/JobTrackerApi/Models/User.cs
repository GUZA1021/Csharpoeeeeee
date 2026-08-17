using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobTrackerApi.Models
{
    public class User
    {
        public int Id { get; private set; }
        [Required]
        public string Email { get;  set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }


    

    public User(String email, String userName, string passwordHash)
    {
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
    }
    
    }
}
