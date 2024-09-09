using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class GameType
{
    public int IdgameType { get; set; }

    public string? GameTypeName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Game> Games { get; } = new List<Game>();
}
