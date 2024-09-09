using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class UserGamerVM
    {
        [Display(Name = "ID")]
        public int IduserGamer { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? LastName { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string? Email { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "Provide a correct city")]
        public string? City { get; set; }
        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Provide a correct postal code")]
        public string? PostalCode { get; set; }
        [Display(Name = "Home Address")]
        [Required(ErrorMessage = "Provide a correct home address")]
        public string? HomeAddress { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string? Password { get; set; }

        public string? PwdHash { get; set; }

        public string? PwdSalt { get; set; }

        public int? UserRoleId { get; set; }
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }
    }
}
