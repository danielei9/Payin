namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingVisibilityToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Visibility", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Visibility");
        }
    }
}
