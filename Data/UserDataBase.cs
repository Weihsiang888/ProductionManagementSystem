using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication7.Data
{
    public class UserDataBase
    {
        [Required(ErrorMessage = "The Username value should be specified.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "The Password value should be specified.")]
        [MinPasswordLength(6, "The Password must be at least 6 characters long.")]
        public string Password { get; set; }

        public string Group { get; set; }

    }
    public class UserData : UserDataBase
    {
        [Required(ErrorMessage = "The Email value should be specified.")]
        [Email(ErrorMessage = "The Email value is invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Phone value should be specified.")]
        public string Notes { get; set; }
    }
}
