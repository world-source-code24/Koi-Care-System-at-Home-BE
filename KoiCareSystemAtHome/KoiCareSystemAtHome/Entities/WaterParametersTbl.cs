using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class WaterParametersTbl
{
    public int ParameterId { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Salt { get; set; }

    public decimal? PhLevel { get; set; }

    public decimal? O2Level { get; set; }

    public decimal? No2Level { get; set; }

    public decimal? No3Level { get; set; }

    public decimal? Po4Level { get; set; }

    public decimal? Nh4Level { get; set; }

    public decimal? KhLevel { get; set; }

    public decimal? GhLevel { get; set; }

    public decimal? Co2Level { get; set; }

    public decimal? TotalChlorines { get; set; }

    public decimal? OutDoorTemp { get; set; }

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public int PondId { get; set; }

    public virtual PondsTbl Pond { get; set; } = null!;
}
