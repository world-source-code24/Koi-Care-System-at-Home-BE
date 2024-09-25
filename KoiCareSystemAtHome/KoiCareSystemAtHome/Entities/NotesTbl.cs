using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class NotesTbl
{
    public int NoteId { get; set; }

    public string? NoteName { get; set; }

    public string NoteText { get; set; } = null!;

    public int UserId { get; set; }

    public virtual UserTbl User { get; set; } = null!;
}
