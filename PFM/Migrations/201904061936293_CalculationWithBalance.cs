namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalculationWithBalance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calculations", "BalanceIncluded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calculations", "BalanceIncluded");
        }
    }
}
