using GameCriticBL.Models;
using System.ComponentModel.DataAnnotations;

namespace GameCritic.DTOModels
{
    public class GameDto
    {
        public int Idgame { get; set; }
        [Required(ErrorMessage = "Game name is required.")]
        public string? GameName { get; set; }
        public string? Description { get; set; }
        public int? Size { get; set; }
        public int? GameTypeId { get; set; }
    }
}
