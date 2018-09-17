namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteView : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Currency_View");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Currency_View",
                c => new
                    {
                        Currency = c.String(nullable: false, maxLength: 3, fixedLength: true),
                    })
                .PrimaryKey(t => t.Currency);
            
        }
    }
}
