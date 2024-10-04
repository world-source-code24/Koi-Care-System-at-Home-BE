using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class MembershipsTbl
{
    public int MembershipId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int AccId { get; set; }

    public virtual AccountTbl Acc { get; set; } = null!;
}
