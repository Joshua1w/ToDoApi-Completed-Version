using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
