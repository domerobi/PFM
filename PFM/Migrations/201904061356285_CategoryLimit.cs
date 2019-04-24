namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCategories", "Limit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCategories", "Limit");
        }
    }
}
