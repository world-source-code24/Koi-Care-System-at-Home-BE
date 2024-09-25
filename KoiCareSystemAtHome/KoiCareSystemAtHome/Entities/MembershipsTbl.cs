using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class MembershipsTbl
{
    public int MembershipId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int UserId { get; set; }

    public virtual UserTbl User { get; set; } = null!;
}
