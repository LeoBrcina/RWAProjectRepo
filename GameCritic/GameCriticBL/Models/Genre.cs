using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class Genre
{
    public int Idgenre { get; set; }

    public string? GenreName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<GameGenre> GameGenres { get; } = new List<GameGenre>();
}
