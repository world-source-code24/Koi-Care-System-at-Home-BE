using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class ShopsTbl
{
    public int ShopId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<ProductsTbl> ProductsTbls { get; set; } = new List<ProductsTbl>();
}
