using System.ComponentModel.DataAnnotations;

namespace JobTrackerApi.Authentication
{
    public class LoginRequest

    {
        [Required]
        public string InputEmail { get; set; }
        [Required]
        public string InputPassword { get; set; }
    }
}
