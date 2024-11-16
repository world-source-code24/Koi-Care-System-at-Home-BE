using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class KoiGrowthChartsTbl
{
    public int ChartId { get; set; }

    public DateTime? Date { get; set; }

    public decimal Length { get; set; }

    public decimal Weight { get; set; }

    public string? HealthStatus { get; set; }

    public int KoiId { get; set; }

    public virtual KoisTbl Koi { get; set; } = null!;
}
