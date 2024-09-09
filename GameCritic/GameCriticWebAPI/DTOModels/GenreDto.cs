using GameCriticBL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameCritic.DTOModels
{
    public class GenreDto
    {
        public int Idgenre { get; set; }
        [Required(ErrorMessage = "Genre name is required.")]
        public string? GenreName { get; set; }
        [Required(ErrorMessage = "Genre description is required.")]
        public string? Description { get; set; }

    }
}
