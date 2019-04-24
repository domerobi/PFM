    namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LimitDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserCategories", "Limit", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserCategories", "Limit", c => c.Int(nullable: false));
        }
    }
}
