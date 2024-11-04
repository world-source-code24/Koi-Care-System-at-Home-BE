using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class OrdersTbl
{
    public int OrderId { get; set; }

    public DateOnly Date { get; set; }

    public string? StatusOrder { get; set; }

    public string? StatusPayment { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? AccId { get; set; }

    public virtual AccountTbl? Acc { get; set; }

    public virtual ICollection<OrderDetailsTbl> OrderDetailsTbls { get; set; } = new List<OrderDetailsTbl>();
}
