using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class ProductsTbl
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }

    public string Category { get; set; } = null!;

    public string? ProductInfo { get; set; }

    public bool? Status { get; set; }

    public int? ShopId { get; set; }

    public virtual ICollection<CartTbl> CartTbls { get; set; } = new List<CartTbl>();

    public virtual ICollection<OrderDetailsTbl> OrderDetailsTbls { get; set; } = new List<OrderDetailsTbl>();

    public virtual ShopsTbl? Shop { get; set; }
}
