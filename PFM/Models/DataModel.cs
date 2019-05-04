namespace PFM.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// The context of the database
    /// </summary>
    public partial class DataModel : DbContext
    {
        /// <summary>
        /// Initialize the name
        /// </summary>
        public DataModel()
            : base("name=DataModel")
        {
        }

        /// <summary>
        /// Set of accounts
        /// </summary>
        public virtual DbSet<Accounts> Accounts { get; set; }

        /// <summary>
        /// Set of Categories
        /// </summary>
        public virtual DbSet<Categories> Categories { get; set; }

        /// <summary>
        /// Set of category directions
        /// </summary>
        public virtual DbSet<CategoryDirections> CategoryDirections { get; set; }

        /// <summary>
        /// Set of exchange rates
        /// </summary>
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }

        /// <summary>
        /// Set of exchange rate histories
        /// </summary>
        public virtual DbSet<ExchangeRateHist> ExchangeRateHist { get; set; }

        /// <summary>
        /// Set of transactions
        /// </summary>
        public virtual DbSet<Transactions> Transactions { get; set; }

        /// <summary>
        /// Set of users
        /// </summary>
        public virtual DbSet<Users> Users { get; set; }

        /// <summary>
        /// Set of accounts with their actual balances
        /// </summary>
        public virtual DbSet<AccountBalance> AccountBalance { get; set; }

        /// <summary>
        /// Set of calculations
        /// </summary>
        public virtual DbSet<Calculation> Calculation { get; set; }

        /// <summary>
        /// Set of calculation datas
        /// </summary>
        public virtual DbSet<CalculationData> CalculationData { get; set; }

        /// <summary>
        /// Set of the cross reference between users and categories
        /// </summary>
        public virtual DbSet<UserCategory> UserCategory { get; set; }

        /// <summary>
        /// Initializing relationships between tables
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>()
                .Property(e => e.StartBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Accounts>()
                .Property(e => e.Currency)
                .IsFixedLength();

            modelBuilder.Entity<Accounts>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Accounts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoryDirections>()
                .Property(e => e.DirectionName)
                .IsFixedLength();

            modelBuilder.Entity<CategoryDirections>()
                .HasMany(e => e.Categories)
                .WithRequired(e => e.CategoryDirections)
                .HasForeignKey(e => e.CategoryDirectionID)
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

            modelBuilder.Entity<Transactions>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountBalance>()
                .Property(e => e.Balance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Calculation>()
                .HasMany(e => e.CalculationData)
                .WithRequired(e => e.Calculation)
                .HasForeignKey(e => e.CalculationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserCategory>()
                .HasRequired(e => e.User)
                .WithMany(e => e.UserCategories)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<UserCategory>()
                .HasRequired(e => e.Category)
                .WithMany(e => e.UserCategories)
                .HasForeignKey(e => e.CategoryID);
        }
    }
}
