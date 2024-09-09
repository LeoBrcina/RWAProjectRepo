using System.ComponentModel.DataAnnotations;

namespace GameCritic.DTOModels
{
    public class GameTypeDto
    {
        public int IdGameType { get; set; }
        [Required(ErrorMessage = "Game type name is required.")]
        public string? GameTypeName { get; set; }
        public string? Description { get; set; }
    }
}
