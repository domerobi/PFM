namespace PFM
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<ExchangeRateHist> ExchangeRateHist { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>()
                .Property(e => e.Currency)
                .IsFixedLength();

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Accounts)
                .HasForeignKey(e => e.AccountFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Transactions1)
                .WithRequired(e => e.Accounts1)
                .HasForeignKey(e => e.AccountTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Currency)
                .IsFixedLength();

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.BaseCurrency)
                .IsFixedLength();

            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Rate)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ExchangeRateHist>()
                .Property(e => e.Currency)
                .IsFixedLength();

            modelBuilder.Entity<ExchangeRateHist>()
                .Property(e => e.BaseCurrency)
                .IsFixedLength();

            modelBuilder.Entity<ExchangeRateHist>()
                .Property(e => e.Rate)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Transactions>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);
        }
    }
}
