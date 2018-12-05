namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrCreateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "CreateDate");
        }
    }
}
