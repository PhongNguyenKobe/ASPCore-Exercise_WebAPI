using System;
using System.Collections.Generic;

namespace NIIEPayAPI.Data;

public partial class TransactionHistory
{
    public long Id { get; set; }

    public long TransactionId { get; set; }

    public long AccountId { get; set; }

    public decimal Amount { get; set; }

    public decimal BalanceAfter { get; set; }

    public string? Note { get; set; }

    public DateTime TransactionTime { get; set; }

    public bool IsSender { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
