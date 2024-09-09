using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class GenreVM
    {
        [Display(Name = "ID")]
        public int Idgenre { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Genre name is required.")]
        public string? GenreName { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Genre description is required.")]
        public string? Description { get; set; }
    }
}
