using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class PondsTbl
{
    public int PondId { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public decimal? Depth { get; set; }

    public int? Volume { get; set; }

    public int? DrainCount { get; set; }

    public int? PumpCapacity { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<KoisTbl> KoisTbls { get; set; } = new List<KoisTbl>();

    public virtual UserTbl User { get; set; } = null!;

    public virtual ICollection<WaterParametersTbl> WaterParametersTbls { get; set; } = new List<WaterParametersTbl>();
}
