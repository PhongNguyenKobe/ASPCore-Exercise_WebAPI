using System;
using System.Collections.Generic;

namespace NIIEPayAPI.Data;

public partial class SavingsAccount
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public decimal Amount { get; set; }

    public int TermMonths { get; set; }

    public decimal InterestRate { get; set; }

    public bool AutoRenew { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly MaturityDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
