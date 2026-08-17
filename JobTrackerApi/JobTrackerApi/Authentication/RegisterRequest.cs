using System.ComponentModel.DataAnnotations;

namespace JobTrackerApi.Authentication
{
    public static class RegexPatterns
    {
        public const string Email = @"^[a-zA-Z0-9._\\-]+@[a-zA-Z0-9]+(([\\-]*[a-zA-Z0-9]+)*[.][a-zA-Z0-9]+)+(;[ ]*[a-zA-Z0-9._\\-]+@[a-zA-Z0-9]+(([\\-]*[a-zA-Z0-9]+)*[.][a-zA-Z0-9]+)+)*$";
    }
    public class RegisterRequest
    {
        [Required]
        [RegularExpression(RegexPatterns.Email, ErrorMessage = "Email can only contain alphanumeric characters")]
        [StringLength(255, MinimumLength = 5)]
        public string Email { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set;}
    
    }
} //mangler at lave sådan at du kan lave et nyt password hvis du ahr glemt det, og mangler også at eller vent ja det kun det