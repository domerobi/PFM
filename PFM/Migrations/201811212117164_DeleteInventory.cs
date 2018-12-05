namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteInventory : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Inventory");
        }
        
        public override void Down()
        {
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
    }
}
