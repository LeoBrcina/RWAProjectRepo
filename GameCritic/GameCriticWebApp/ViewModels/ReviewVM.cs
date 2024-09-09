using System.ComponentModel.DataAnnotations;

namespace GameCriticWebApp.ViewModels
{
    public class ReviewVM
    {
        [Display(Name = "ID")]
        public int Idreview { get; set; }
        [Display(Name = "Game ID")]
        public int? GameId { get; set; }
        [Display(Name = "Gamer ID")]
        public int? GamerId { get; set; }
        [Display(Name = "Rating")]
        [Range(1, 100)]
        public int? Rating { get; set; }
        [Display(Name = "Text")]
        [Required(ErrorMessage = "Review text is required.")]
        public string? ReviewText { get; set; }
        [Display(Name = "Game Name")]
        public string? GameName { get; set; }
        [Display(Name = "Username")]
        public string? Username { get; set; }

    }
}
