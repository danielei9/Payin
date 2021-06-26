namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingattributeCodetoProductFamilyProductEntranceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "Code", c => c.String(nullable: false));
            AddColumn("dbo.Products", "Code", c => c.String(nullable: false));
            AddColumn("dbo.ProductFamilies", "Code", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductFamilies", "Code");
            DropColumn("dbo.Products", "Code");
            DropColumn("dbo.EntranceTypes", "Code");
        }
    }
}
