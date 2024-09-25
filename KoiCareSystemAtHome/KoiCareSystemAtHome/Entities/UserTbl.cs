using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class UserTbl
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<MembershipsTbl> MembershipsTbls { get; set; } = new List<MembershipsTbl>();

    public virtual ICollection<NotesTbl> NotesTbls { get; set; } = new List<NotesTbl>();

    public virtual ICollection<OrdersTbl> OrdersTbls { get; set; } = new List<OrdersTbl>();

    public virtual ICollection<PondsTbl> PondsTbls { get; set; } = new List<PondsTbl>();
}
