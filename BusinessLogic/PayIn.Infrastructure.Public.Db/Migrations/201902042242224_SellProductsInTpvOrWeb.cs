namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SellProductsInTpvOrWeb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SellableInTpv", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Products", "SellableInWeb", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SellableInWeb");
            DropColumn("dbo.Products", "SellableInTpv");
        }
    }
}
