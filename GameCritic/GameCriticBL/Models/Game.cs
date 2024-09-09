using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class Game
{
    public int Idgame { get; set; }

    public string? GameName { get; set; }

    public int? GameTypeId { get; set; }

    public string? Description { get; set; }

    public int? Size { get; set; }

    public virtual ICollection<GameGenre> GameGenres { get; } = new List<GameGenre>();

    public virtual GameType? GameType { get; set; }

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();
}
