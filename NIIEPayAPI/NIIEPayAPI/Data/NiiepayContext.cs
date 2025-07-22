using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NIIEPayAPI.Data;

public partial class NiiepayContext : DbContext
{
    public NiiepayContext()
    {
    }

    public NiiepayContext(DbContextOptions<NiiepayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<InterestRate> InterestRates { get; set; }

    public virtual DbSet<SavingsAccount> SavingsAccounts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-3KJ6M8N4\\SQLEXPRESS;Initial Catalog=NIIEPay;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accounts__3213E83F078E30E6");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Accounts__A1936A6B8BD37ADC").IsUnique();

            entity.HasIndex(e => e.AccountNumber, "UQ__Accounts__AF91A6AD27DAAA5B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(100)
                .HasColumnName("account_holder_name");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("account_number");
            entity.Property(e => e.AvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("available_balance");
            entity.Property(e => e.CitizenId)
                .HasMaxLength(20)
                .HasColumnName("citizen_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdExpiryDate).HasColumnName("id_expiry_date");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<InterestRate>(entity =>
        {
            entity.HasKey(e => e.TermMonths).HasName("PK__Interest__00FBF1F1A2380D9A");

            entity.Property(e => e.TermMonths)
                .ValueGeneratedNever()
                .HasColumnName("term_months");
            entity.Property(e => e.InterestRate1)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("interest_rate");
        });

        modelBuilder.Entity<SavingsAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SavingsA__3213E83F3A913689");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AutoRenew).HasColumnName("auto_renew");
            entity.Property(e => e.InterestRate)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("interest_rate");
            entity.Property(e => e.MaturityDate).HasColumnName("maturity_date");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("open")
                .HasColumnName("status");
            entity.Property(e => e.TermMonths).HasColumnName("term_months");

            entity.HasOne(d => d.Account).WithMany(p => p.SavingsAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SavingsAc__accou__5EBF139D");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3213E83FCD3B5C4A");

            entity.HasIndex(e => e.TransactionId, "UQ__Transact__85C600AEFD756A09").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.FromAccount).HasColumnName("from_account");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.ToAccount).HasColumnName("to_account");
            entity.Property(e => e.ToBankCode)
                .HasMaxLength(10)
                .HasColumnName("to_bank_code");
            entity.Property(e => e.ToPhone)
                .HasMaxLength(15)
                .HasColumnName("to_phone");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .HasColumnName("transaction_id");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.FromAccountNavigation).WithMany(p => p.TransactionFromAccountNavigations)
                .HasForeignKey(d => d.FromAccount)
                .HasConstraintName("FK__Transacti__from___5629CD9C");

            entity.HasOne(d => d.ToAccountNavigation).WithMany(p => p.TransactionToAccountNavigations)
                .HasForeignKey(d => d.ToAccount)
                .HasConstraintName("FK__Transacti__to_ac__571DF1D5");
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3213E83FBFA4F7E4");

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BalanceAfter)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("balance_after");
            entity.Property(e => e.IsSender).HasColumnName("is_sender");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.TransactionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_time");

            entity.HasOne(d => d.Account).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__accou__5AEE82B9");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__trans__59FA5E80");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
