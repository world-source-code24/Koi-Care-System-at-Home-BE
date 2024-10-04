using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class NotesTbl
{
    public int NoteId { get; set; }

    public string? NoteName { get; set; }

    public string NoteText { get; set; } = null!;

    public int AccId { get; set; }

    public virtual AccountTbl Acc { get; set; } = null!;
}
