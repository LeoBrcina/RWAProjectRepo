using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class Review
{
    public int Idreview { get; set; }

    public int? GameId { get; set; }

    public int? GamerId { get; set; }

    public int? Rating { get; set; }

    public string? ReviewText { get; set; }

    public virtual Game? Game { get; set; }

    public virtual UserGamer? Gamer { get; set; }
}
