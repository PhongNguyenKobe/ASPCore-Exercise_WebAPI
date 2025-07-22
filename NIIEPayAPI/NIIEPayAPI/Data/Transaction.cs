using System;
using System.Collections.Generic;

namespace NIIEPayAPI.Data;

public partial class Transaction
{
    public long Id { get; set; }

    public string TransactionId { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public long? FromAccount { get; set; }

    public long? ToAccount { get; set; }

    public string? ToPhone { get; set; }

    public string? ToBankCode { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Account? FromAccountNavigation { get; set; }

    public virtual Account? ToAccountNavigation { get; set; }

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
