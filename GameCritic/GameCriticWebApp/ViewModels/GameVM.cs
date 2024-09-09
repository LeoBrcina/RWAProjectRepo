using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class GameVM
    {
        [Display(Name = "ID")]
        public int Idgame { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Game name is required.")]
        public string? GameName { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Game description is required.")]
        public string? Description { get; set; }
        [Display(Name = "Size")]
        [Range(1, 250)]
        public int? Size { get; set; }
        [Display(Name = "Game Type ID")]
        [Required(ErrorMessage = "Game type is required.")]
        public int? GameTypeId { get; set; }
        [Display(Name = "Game Type Name")]
        public string? GameTypeName { get; set; }
        [Display(Name = "Genres (Hold CTRL to select more than 1 genre.)")]
        [Required(ErrorMessage = "Atleast one genre is required.")]
        public List<int> GenreIds { get; set; } = new List<int>();
        [Display(Name = "Genre Names")]
        public List<string> GenreNames { get; set; } = new List<string>();
    }
}
