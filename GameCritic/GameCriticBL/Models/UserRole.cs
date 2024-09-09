using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class UserRole
{
    public int IduserRole { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<UserGamer> UserGamers { get; } = new List<UserGamer>();
}
