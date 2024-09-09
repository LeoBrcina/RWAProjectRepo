using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string? Password { get; set; }
        public string ReturnUrl{ get; set; }
    }
}
