using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class GameGenre
{
    public int IdgameGenre { get; set; }

    public int? GameId { get; set; }

    public int GenreId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Genre? Genre { get; set; }
}
