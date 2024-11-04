using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class MembershipDashboard
{
    public int Id { get; set; }

    public decimal Money { get; set; }

    public DateOnly StartDate { get; set; }

    public int AccId { get; set; }

    public virtual AccountTbl Acc { get; set; } = null!;
}
