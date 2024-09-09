using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class UserGamer
{
    public int IduserGamer { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? HomeAddress { get; set; }

    public string? Username { get; set; }

    public string? PwdHash { get; set; }

    public string? PwdSalt { get; set; }

    public int? UserRoleId { get; set; }

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual UserRole? UserRole { get; set; }
}
