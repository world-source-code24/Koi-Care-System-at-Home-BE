using System;
using System.Collections.Generic;

namespace KoiCareSystemAtHome.Entities;

public partial class RefreshToken
{
    public string TokenId { get; set; } = null!;

    public int? AccId { get; set; }

    public string? Token { get; set; }

    public string? JwtId { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? IssueAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual AccountTbl? Acc { get; set; }
}
