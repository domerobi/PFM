namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Outdated = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                        Currency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        AccountName = c.String(nullable: false, maxLength: 50),
                        LastModify = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        TransactionDate = c.DateTime(nullable: false, storeType: "date"),
                        AccountFrom = c.Int(nullable: false),
                        AccountTo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Accounts", t => t.AccountFrom)
                .ForeignKey("dbo.Accounts", t => t.AccountTo)
                .Index(t => t.CategoryID)
                .Index(t => t.AccountFrom)
                .Index(t => t.AccountTo);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        Direction = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Outdated = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 64),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Currency_View",
                c => new
                    {
                        Currency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                    })
                .PrimaryKey(t => t.Currency);
            
            CreateTable(
                "dbo.ExchangeRate",
                c => new
                    {
                        Currency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        BaseCurrency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        Rate = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.Currency);
            
            CreateTable(
                "dbo.ExchangeRateHist",
                c => new
                    {
                        Currency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        ValidFrom = c.DateTime(nullable: false, storeType: "date"),
                        BaseCurrency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        Rate = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ValidTo = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => new { t.Currency, t.ValidFrom });
            
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sum = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Comment = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Transactions", "AccountTo", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "AccountFrom", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] { "AccountTo" });
            DropIndex("dbo.Transactions", new[] { "AccountFrom" });
            DropIndex("dbo.Transactions", new[] { "CategoryID" });
            DropIndex("dbo.Accounts", new[] { "UserID" });
            DropTable("dbo.Inventory");
            DropTable("dbo.ExchangeRateHist");
            DropTable("dbo.ExchangeRate");
            DropTable("dbo.Currency_View");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
            DropTable("dbo.Transactions");
            DropTable("dbo.Accounts");
        }
    }
}
