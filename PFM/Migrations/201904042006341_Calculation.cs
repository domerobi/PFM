namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Calculation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calculations",
                c => new
                    {
                        CalculationID = c.Int(nullable: false, identity: true),
                        AccountID = c.Int(nullable: false),
                        CalculationName = c.String(maxLength: 40),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        DueDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalculationID)
                .ForeignKey("dbo.Accounts", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.CalculationDatas",
                c => new
                    {
                        CalculationDataID = c.Int(nullable: false, identity: true),
                        CalculationID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        Average = c.Decimal(nullable: false, storeType: "money"),
                        Limit = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.CalculationDataID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Calculations", t => t.CalculationID)
                .Index(t => t.CalculationID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CalculationDatas", "CalculationID", "dbo.Calculations");
            DropForeignKey("dbo.CalculationDatas", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Calculations", "AccountID", "dbo.Accounts");
            DropIndex("dbo.CalculationDatas", new[] { "CategoryID" });
            DropIndex("dbo.CalculationDatas", new[] { "CalculationID" });
            DropIndex("dbo.Calculations", new[] { "AccountID" });
            DropTable("dbo.CalculationDatas");
            DropTable("dbo.Calculations");
        }
    }
}
