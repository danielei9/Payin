namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsVisibleForProductsAndEvents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "IsVisible", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Events", "IsVisible", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Products", "IsVisible", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ProductFamilies", "IsVisible", c => c.Boolean(nullable: false, defaultValue: true));
		}
        
        public override void Down()
        {
            DropColumn("dbo.ProductFamilies", "IsVisible");
            DropColumn("dbo.Products", "IsVisible");
            DropColumn("dbo.Events", "IsVisible");
            DropColumn("dbo.EntranceTypes", "IsVisible");
        }
    }
}
