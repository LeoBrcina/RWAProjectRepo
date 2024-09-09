using System.ComponentModel.DataAnnotations;

namespace GameCritic.DTOModels
{
    public class ReviewDto
    {
        public int Idreview { get; set; }

        public int? GameId { get; set; }

        public int? GamerId { get; set; }
        [Range(1, 100)]
        public int? Rating { get; set; }
        [Required(ErrorMessage = "Review text is required.")]
        public string? ReviewText { get; set; }
    }
}
