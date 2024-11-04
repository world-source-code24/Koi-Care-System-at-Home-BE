using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class CartTbl
{
    public int AccId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual AccountTbl Acc { get; set; } = null!;

    public virtual ProductsTbl Product { get; set; } = null!;
}
