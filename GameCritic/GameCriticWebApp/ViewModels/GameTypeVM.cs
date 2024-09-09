using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class GameTypeVM
    {
        [Display(Name ="ID")]
        public int IdgameType { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Game type name is required.")]
        public string? GameTypeName { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Game type description is required.")]
        public string? Description { get; set; }
    }
}
