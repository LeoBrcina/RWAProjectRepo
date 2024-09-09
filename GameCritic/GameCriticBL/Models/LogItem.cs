using System;
using System.Collections.Generic;

namespace GameCriticBL.Models;

public partial class LogItem
{
    public int Idlog { get; set; }

    public DateTime? Tmstamp { get; set; }

    public int? Lvl { get; set; }

    public string? Txt { get; set; }
}
