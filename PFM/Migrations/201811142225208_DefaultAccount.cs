namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Default", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Default");
        }
    }
}
