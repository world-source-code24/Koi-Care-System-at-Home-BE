using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class OrdersTbl
{
    public int OrderId { get; set; }

    public DateOnly Date { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<OrderDetailsTbl> OrderDetailsTbls { get; set; } = new List<OrderDetailsTbl>();

    public virtual UserTbl? User { get; set; }
}
