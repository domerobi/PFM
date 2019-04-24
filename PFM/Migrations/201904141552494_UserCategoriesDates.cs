namespace PFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCategoriesDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCategories", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserCategories", "LastModify", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCategories", "LastModify");
            DropColumn("dbo.UserCategories", "CreateDate");
        }
    }
}
