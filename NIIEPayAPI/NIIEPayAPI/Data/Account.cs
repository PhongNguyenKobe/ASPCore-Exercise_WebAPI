using System;
using System.Collections.Generic;

namespace NIIEPayAPI.Data;

public partial class Account
{
    public long Id { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string CitizenId { get; set; } = null!;

    public DateOnly IdExpiryDate { get; set; }

    public decimal AvailableBalance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<SavingsAccount> SavingsAccounts { get; set; } = new List<SavingsAccount>();

    public virtual ICollection<Transaction> TransactionFromAccountNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();

    public virtual ICollection<Transaction> TransactionToAccountNavigations { get; set; } = new List<Transaction>();
}
