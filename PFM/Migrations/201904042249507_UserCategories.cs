namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCategories",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ExcludeFromCalculation = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.CategoryID })
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CategoryID);
            
            AddColumn("dbo.Categories", "Default", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCategories", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserCategories", "CategoryID", "dbo.Categories");
            DropIndex("dbo.UserCategories", new[] { "CategoryID" });
            DropIndex("dbo.UserCategories", new[] { "UserID" });
            DropColumn("dbo.Categories", "Default");
            DropTable("dbo.UserCategories");
        }
    }
}
