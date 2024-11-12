using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class FoodCalculateParameter
{
    public int ParameterId { get; set; }

    public string? Level { get; set; }

    public int? RangeId { get; set; }

    public double? MultiplierLower { get; set; }

    public double? MultiplierBetween { get; set; }

    public double? MultiplierUpper { get; set; }

    public string? Advice { get; set; }

    public virtual TemperatureRange? Range { get; set; }
}
