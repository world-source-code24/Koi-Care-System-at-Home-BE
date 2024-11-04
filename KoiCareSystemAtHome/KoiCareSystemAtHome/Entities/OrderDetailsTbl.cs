using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class OrderDetailsTbl
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual OrdersTbl Order { get; set; } = null!;

    public virtual ProductsTbl Product { get; set; } = null!;
}
