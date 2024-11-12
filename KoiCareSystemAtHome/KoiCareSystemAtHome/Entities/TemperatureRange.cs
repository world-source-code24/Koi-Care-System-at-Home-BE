using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class TemperatureRange
{
    public int RangeId { get; set; }

    public decimal? MinTemp { get; set; }

    public decimal? MaxTemp { get; set; }

    public virtual ICollection<FoodCalculateParameter> FoodCalculateParameters { get; set; } = new List<FoodCalculateParameter>();
}
