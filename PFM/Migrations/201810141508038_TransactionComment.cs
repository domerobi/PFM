namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Comment", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Comment");
        }
    }
}
